// See https://aka.ms/new-console-template for more information
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using DayMemory.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using File = DayMemory.Core.Models.Personal.File;
using DayMemory.Core.Models.Common;
using System.Net.Http.Json;
using System.Linq;
using DayMemory.Console.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;
using System.Text.RegularExpressions;
using Azure.Core;
using Newtonsoft.Json;
using System.IO;
using MediatR;
using System.Net.Http.Headers;
using DayMemory.Core.Queries.Projections;

var builder = new ConfigurationBuilder()
 .AddJsonFile($"appsettings.Development.json", true, true);
var config = builder.Build();


var storageConnectionString = config["AppUrls:StorageConnectionString"];
var connectionString = config["ConnectionStrings:DefaultConnection"];
var destContainerName = config["FileStorage:Container"];

var optionsBuilder = new DbContextOptionsBuilder<DayMemoryDbContext>();
optionsBuilder.UseSqlServer(connectionString);
var dbContext = new DayMemoryDbContext(optionsBuilder.Options);

//await CleanFiles(storageConnectionString, destContainerName, dbContext);
//await ResizeImages(storageConnectionString, destContainerName, dbContext);
await PrepareDemoAccount(dbContext, config);
Console.WriteLine("End");



async Task PrepareDemoAccount(DayMemoryDbContext dbContext, IConfigurationRoot config)
{
    var notebookId = config["DemoAccount:MainNotebookId"];
    var userId = config["DemoAccount:UserId"];
    var userEmail = config["DemoAccount:UserEmail"];
    var userPassword = config["DemoAccount:UserPassword"];
    var apiUrl = config["AppUrls:ApiUrl"];

    using var client = new HttpClient();

    //1. Login
    var httpResult = await client.PostAsJsonAsync($"{apiUrl}/api/account/login", new { Email = userEmail, Password = userPassword });
    var accountResponse = await httpResult.Content.ReadFromJsonAsync<AccountResponse>();

    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accountResponse!.AccessToken);

    //2. Get and delete notes
    var notes = await client.GetFromJsonAsync<List<NoteItemProjection>>($"{apiUrl}/api/notes?notebookId={notebookId}") ?? new List<NoteItemProjection>();
    foreach (var note in notes)
    {
        await client.DeleteAsync($"{apiUrl}/api/notes/{note.Id}");
    }

    // 3. Create notes
    var startDate = DateTime.UtcNow.AddYears(-1);
    await client.PostAsJsonAsync($"{apiUrl}/api/notes", new
    {
        NoteId = Guid.NewGuid(),
        NotebookId = notebookId,
        Text = "Daily writing has become an essential part of my routine. It allows me to clear my mind, reflect on my day, and explore my inner thoughts and feelings. Through consistent writing, I have noticed improvements in my writing skills, increased self-awareness, and a greater sense of calm and clarity. Daily writing has become a valuable tool for personal growth and self-discovery.",
        Date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
    });

    using var content = new MultipartFormDataContent();

    var fileContent = new StreamContent(new FileStream("images/1.jpg", FileMode.Open));
    fileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("image/jpg");
    content.Add(fileContent, "file", "1.jpg");

    content.Add(new StringContent("1170"), name: "Width");
    content.Add(new StringContent("780"), name: "Height");
    content.Add(new StringContent(Guid.NewGuid().ToString()), name: "FileId");
    content.Add(new StringContent(FileType.Image.ToString()), name: "FileType");

    var fileResult = await client.PostAsync($"{apiUrl}/api/files/media", content);

    var file = await fileResult.Content.ReadFromJsonAsync<FileProjection>();



    var newNoteResult = await client.PostAsJsonAsync($"{apiUrl}/api/notes", new
    {
        NoteId = Guid.NewGuid(),
        NotebookId = notebookId,
        Text = "Watching the sunset with my wife and son, I am overwhelmed with gratitude for our little family and the beauty of the world around us.",
        Date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
        MediaFiles = new string[] { file!.Id }

    });
    var newNote = await newNoteResult.Content.ReadFromJsonAsync<NoteItemProjection>();

    Console.Write(accountResponse);

}

async Task ResizeImages(string? storageConnectionString, string? containerName, DayMemoryDbContext dbContext)
{
    var users = await dbContext.Set<User>().ToListAsync();

    foreach (var user in users)
    {
        var files = await dbContext.Set<File>().Where(x => x.UserId == user.Id).ToListAsync();
        foreach (var item in files.Where(x => x.FileType == FileType.Image))
        {
            var sourceBlobContainerClient = new BlobContainerClient(storageConnectionString, containerName);
            BlobClient sourceBlob = sourceBlobContainerClient.GetBlobClient($"{user.Id}/{item.Id}/original");

            var destBlobContainerClient = new BlobContainerClient(storageConnectionString, containerName);
            BlobClient destBlob = destBlobContainerClient.GetBlobClient($"{user.Id}/{item.Id}/resized");

            if (await sourceBlob.ExistsAsync() /*&& !await destBlob.ExistsAsync()*/)
            {
                if (await destBlob.ExistsAsync())
                {
                    await destBlob.DeleteAsync();
                }

                using (var stream = new MemoryStream())
                {
                    await sourceBlob.DownloadToAsync(stream);
                    stream.Position = 0;

                    try
                    {
                        var result = await new ImageService().ResizeImageAsync(stream, 1500, 70);
                        await destBlob.UploadAsync(result.Stream, new BlobUploadOptions() { HttpHeaders = new BlobHttpHeaders() { ContentType = "image/jpeg" } });
                        result.Stream.Dispose();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + item.Id);
                    }
                }
            }
            Console.WriteLine(item.Id);
        }

    }
}

static async Task MigrateFiles(string? storageConnectionString, string? destContainerName, DayMemoryDbContext dbContext)
{
    var users = await dbContext.Set<User>().ToListAsync();

    foreach (var user in users)
    {
        var files = await dbContext.Set<File>().Where(x => x.UserId == user.Id).ToListAsync();
        foreach (var item in files)
        {
            var sourceContainerName = "note-images";
            var sourceBlobContainerClient = new BlobContainerClient(storageConnectionString, sourceContainerName);
            BlobClient sourceBlob = sourceBlobContainerClient.GetBlobClient($"{user.Id}/{item.Id}");

            var destBlobContainerClient = new BlobContainerClient(storageConnectionString, destContainerName);
            BlobClient destBlob = destBlobContainerClient.GetBlobClient($"{user.Id}/{item.Id}/original");

            if (await sourceBlob.ExistsAsync())
            {
                if (!await destBlob.ExistsAsync())
                {
                    await destBlob.StartCopyFromUriAsync(sourceBlob.Uri);
                }
            }
            Console.WriteLine(item.Id);
        }

    }
}

static async Task CleanFiles(string? storageConnectionString, string? destContainerName, DayMemoryDbContext dbContext)
{

    var files = await dbContext.Set<File>().ToListAsync();
    foreach (var item in files)
    {
        if (await dbContext.Set<NoteFile>().AnyAsync(x => x.FileId == item.Id))
        {
            continue;
        }

        var sourceBlobContainerClient = new BlobContainerClient(storageConnectionString, destContainerName);
        BlobClient sourceBlob = sourceBlobContainerClient.GetBlobClient($"{item.UserId}/{item.Id}/original");

        if (await sourceBlob.ExistsAsync())
        {
            await sourceBlob.DeleteAsync();
        }
        Console.WriteLine(item.Id);
    }


}