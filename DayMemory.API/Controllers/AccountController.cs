using DayMemory.Core.Notifications;
using DayMemory.Core.Settings;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Services;
using DayMemory.Web.Api.Models;
using DayMemory.Web.Components.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Web;

namespace DayMemory.Web.Areas.Mobile
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IJwTokenHelper _jwTokenHelper;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UrlSettings _urlSettings;
        private readonly IMediator _mediator;
        private readonly IEmailTemplateGenerator _emailTemplateGenerator;

        public AccountController(UserManager<IdentityUser> userManager, ILogger<AccountController> logger,
            IJwTokenHelper jwTokenHelper, IEmailSender emailSender, IConfiguration configuration, SignInManager<IdentityUser> signInManager, UrlSettings urlSettings, 
            IMediator mediator, IEmailTemplateGenerator emailTemplateGenerator)
        {

            _logger = logger;
            _jwTokenHelper = jwTokenHelper;
            _emailSender = emailSender;
            _configuration = configuration;
            _signInManager = signInManager;
            _urlSettings = urlSettings;
            _mediator = mediator;
            _emailTemplateGenerator = emailTemplateGenerator;
            _userManager = userManager;
        }


        [HttpPost]
        [Route("api/account/change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.NewPassword != model.ConfirmationPassword)
            {
                return BadRequest("Password confirmation does not match with the Password");
            }

            var userId = User.Identity!.Name;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }

            var passChangeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.ConfirmationPassword);
            if (!passChangeResult.Succeeded)
            {
                return BadRequest(passChangeResult.Errors);
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/account/restore-password")]
        public async Task<ActionResult> RestorePassword(RestorePasswordInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("No user with the specified e-mail found!");
            }

            var res = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!res.Succeeded)
            {
                return BadRequest("Can't restore password. Please contact Administrator.");
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/account/forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var backOfficeTemplateUrl = _urlSettings.RestorePasswordUrlTemplate;
            var restoreLinkUrl = string.Format(backOfficeTemplateUrl!, HttpUtility.UrlEncode(token), model.Email);

            //SEND EMAIL
            var content = _emailTemplateGenerator.GenerateMailTemplate("RestorePassword", "en",
                new Dictionary<string, string>() {
                                { "RestoreLinkUrl", restoreLinkUrl }
                });

            _emailSender.SendMail(model.Email!, "Password Recovery", content);

            return Ok();
        }


        [HttpPost]
        [Route("api/account/login")]
        public async Task<ActionResult> Login(LoginInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email) as User;
            if (user == null)
            {
                return BadRequest();
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return BadRequest();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = await _jwTokenHelper.GenerateJwtToken(user);

            return Ok(new
            {
                Token = token,
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdministrator = roles.Any(x => x == "Administrator")
            });
        }

        [HttpPost]
        [Route("api/account/emailcheck")]
        public async Task<ActionResult<AccountModel>> CheckEmail(CheckEmailInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email) as User;
            if (user != null)
            {
                return BadRequest("User with such e-mail already exists");
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/account/signup")]
        public async Task<ActionResult<AccountModel>> Signup(SignupInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email) as User;
            if (user != null)
            {
                ModelState.AddModelError("Email", "User with such e-mail already exists");
                return BadRequest(ModelState);
            }

            user = new User { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(ModelState);
            }
            user = await _userManager.FindByEmailAsync(model.Email) as User;

            var token = await _jwTokenHelper.GenerateJwtToken(user!);

            await _mediator.Publish(new UserCreatedNotification() { UserId = user!.Id });

            return Ok(new
            {
                Token = token,
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }

        [HttpPost]
        [Route("api/account/signup/social")]
        public async Task<ActionResult> SocialSignup(SocialSignupInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByLoginAsync(model.ProviderType, model.Id) as User;
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var t = await _jwTokenHelper.GenerateJwtToken(user);
                return Ok(new
                {
                    Token = t,
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("Email", "E-mail is required");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string jsonString = JsonSerializer.Serialize(model);
            _logger.LogInformation("User signup", jsonString);

            user = await _userManager.FindByEmailAsync(model.Email) as User;
            if (user == null)
            {
                user = new User
                {
                    Id = StringUtils.GenerateUniqueString(),
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to create account");
                    return BadRequest(ModelState);
                }

                //if (!string.IsNullOrEmpty(model.ImageUrl))
                //{
                //    await DownloadUserImage(model, user);
                //}
            }

            await _userManager.AddLoginAsync(user, new UserLoginInfo(model.ProviderType, model.Id, model.FirstName + model.LastName));
            var token = await _jwTokenHelper.GenerateJwtToken(user);

            await _mediator.Publish(new UserCreatedNotification() { UserId = user.Id });

            return Ok(new
            {
                Token = token,
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }
    }
}