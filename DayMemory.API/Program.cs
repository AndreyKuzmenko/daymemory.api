using DayMemory.Core.Settings;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Exceptions;
using DayMemory.Core.Services;
using DayMemory.DAL;
using DayMemory.DAL.Repositories;
using DayMemory.Web.Components.Auth;
using DayMemory.Web.Components.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using DayMemory.Core;
using Microsoft.Extensions.Logging;
using DayMemory.API.Components;
using Microsoft.AspNetCore.DataProtection;
using DayMemory.Core.Services.Interfaces;
using Google.Api;
using Hangfire;
using Hangfire.SqlServer;
using X.Extensions.Logging.Telegram;
using Microsoft.AspNetCore.Http.Features;

string CorsPolicyName = "DayMemoryCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    //options.ModelValidatorProviders.Clear();
}).AddJsonOptions(options =>
{
    //options.JsonSerializerOptions.Converters.Add(new LocalizationToJsonConverter());
})
    .AddProblemDetailsConventions();
builder.Services.AddProblemDetails(ConfigureProblemDetails);

//SETTINGS
//https://andrewlock.net/adding-validation-to-strongly-typed-configuration-objects-in-dotnet-6/?utm_source=csharpdigest&utm_medium=email&utm_campaign=436
builder.Services.AddOptions<EmailSenderSettings>()
    .BindConfiguration("EmailSender")
    .ValidateDataAnnotations()
.ValidateOnStart();

builder.Services.AddSingleton(resolver =>
        resolver.GetRequiredService<IOptions<EmailSenderSettings>>().Value);

builder.Services.AddOptions<UrlSettings>()
    .BindConfiguration("AppUrls")
    .ValidateDataAnnotations()
.ValidateOnStart();

builder.Services.AddSingleton(resolver =>
        resolver.GetRequiredService<IOptions<UrlSettings>>().Value);




// SERVICES
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITagRepository, TagRepository>();
builder.Services.AddTransient<INotebookRepository, NotebookRepository>();
builder.Services.AddTransient<IFileRepository, FileRepository>();
builder.Services.AddTransient<ILocationRepository, LocationRepository>();
builder.Services.AddTransient<INoteItemRepository, NoteItemRepository>();
builder.Services.AddTransient<IQuestionListRepository, QuestionListRepository>();
builder.Services.AddTransient<IQuestionRepository, QuestionRepository>();

builder.Services.AddTransient<IJwTokenHelper, JwTokenHelper>();
builder.Services.AddTransient<ISystemClock, SystemClock>();
builder.Services.AddTransient<IFileService, AzureFileService>();
builder.Services.AddTransient<IUrlResolver, UrlResolver>();
builder.Services.AddTransient<IImageService, ImageService>();



// MEDIATR
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(DayMemory.Core.AssemblyInfo).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(DayMemory.DAL.AssemblyInfo).Assembly));

builder.Services.AddTransient<IEmailTemplateGenerator, EmailTemplateGenerator>();

builder.Services.AddTransient<IEmailTemplateGenerator, EmailTemplateGenerator>(i => new EmailTemplateGenerator(
       i.GetRequiredService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>(),
       i.GetRequiredService<ILogger<EmailTemplateGenerator>>()));

builder.Services.AddTransient<IEmailSender, EmailSender>(i => new EmailSender(
        i.GetRequiredService<IOptions<EmailSenderSettings>>().Value,
       i.GetRequiredService<ILogger<EmailSender>>(),
       i.GetRequiredService<IEmailTemplateGenerator>()));

// DATA ACCESS            
builder.Services.AddDbContext<DayMemoryDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IReadDbContext, DayMemoryReadOnlyDbContext>();

// AUTHENTICATION
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DayMemoryDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromMinutes(10));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.AllowedUserNameCharacters = string.Empty;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
});

builder.Services.Configure<FormOptions>(o =>  // currently all set to max, configure it to your needs!
{
    o.MultipartBodyLengthLimit = Constants.RequestLimits.MaxFileSize; //100MB
    o.ValueLengthLimit = (int)Constants.RequestLimits.MaxFileSize; //100MB
});

var tokenKey = builder.Configuration.GetValue<string>("JWT:Secret");
if (tokenKey == null)
{
    throw new ConfigurationException("JWT:Secret");
}
var key = Encoding.ASCII.GetBytes(tokenKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            AuthenticationType = JwtBearerDefaults.AuthenticationScheme,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
        x.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context => {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                }
                return Task.CompletedTask;
            }
        };
    })
    .AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme,
    options =>
    {
        options.LoginPath = "/Account/LogIn"; ;
        options.AccessDeniedPath = new PathString("/account/login");
        options.Cookie.Name = "DayMemory";
        options.ExpireTimeSpan = new TimeSpan(365, 0, 0, 0);
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.Cookie.SameSite = SameSiteMode.None;

    });


var multiSchemePolicy = new AuthorizationPolicyBuilder(
        CookieAuthenticationDefaults.AuthenticationScheme,
        JwtBearerDefaults.AuthenticationScheme)
      .RequireAuthenticatedUser()
      .Build();


builder.Services.AddAuthorization(o =>
{
    o.DefaultPolicy = multiSchemePolicy;
    o.AddPolicy(Constants.AdminPolicy, policy =>
    {
        policy.RequireRole(Constants.AdminRole);
        //policy.AuthenticationSchemes = "";
    });
});

builder.Services.AddApplicationInsightsTelemetry();
builder.Logging.AddTelegram(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DayMemoryDbContext>();
    context.Database.Migrate();
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseCors(CorsPolicyName);
var cookiePolicyOptions = new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.SameAsRequest,
    MinimumSameSitePolicy = SameSiteMode.None
};

app.UseCookiePolicy(cookiePolicyOptions);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    //Authorization = new[] { new AdminAuthorizationFilter() }
});

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("OK");
});

app.MapGet("/env", async context =>
{
    if (context.User.Identity == null || !context.User.Identity.IsAuthenticated || !context.User.IsInRole(Constants.AdminRole))
    {
        await context.Response.WriteAsync("Access Forbidden!");
        return;
    }

    IWebHostEnvironment? hostEnvironment = context.RequestServices.GetRequiredService<IWebHostEnvironment>();
    var thisEnv = new
    {
        ApplicationName = hostEnvironment.ApplicationName,
        Environment = hostEnvironment.EnvironmentName,
    };

    var jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
    await context.Response.WriteAsJsonAsync(thisEnv, jsonSerializerOptions);
});


app.MapGet("/conf", async context =>
{
    if (context.User.Identity == null || !context.User.Identity.IsAuthenticated || !context.User.IsInRole(Constants.AdminRole))
    {
        await context.Response.WriteAsync("Access Forbidden!");
        return;
    }

    IConfiguration allConfig = context.RequestServices.GetRequiredService<IConfiguration>();

    IEnumerable<KeyValuePair<string, string>> configKv = allConfig == null ? Array.Empty<KeyValuePair<string, string>>() : allConfig.AsEnumerable();

    var jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
    await context.Response.WriteAsJsonAsync(configKv, jsonSerializerOptions);
});

app.MapGet("/api/keep-alive", async context =>
{
    await context.Response.WriteAsync("OK");
});

app.MapGet("/api/logging", async context =>
{
    app.Logger.LogError("Error test");
    await context.Response.WriteAsync("OK");
});


app.Logger.LogInformation("App Started");

app.Run();



void ConfigureProblemDetails(Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options)
{
    // Custom mapping function for FluentValidation's ValidationException.
    options.MapFluentValidationException();

    // This will map ResourceNotFoundException to the 404 Not Found status code.
    options.MapToStatusCode<ResourceNotFoundException>(StatusCodes.Status404NotFound);

    // You can configure the middleware to re-throw certain types of exceptions, all exceptions or based on a predicate.
    // This is useful if you have upstream middleware that needs to do additional handling of exceptions.
    options.Rethrow<NotSupportedException>();

    // This will map NotImplementedException to the 501 Not Implemented status code.
    options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

    // This will map HttpRequestException to the 503 Service Unavailable status code.
    options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

    // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
    // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
}