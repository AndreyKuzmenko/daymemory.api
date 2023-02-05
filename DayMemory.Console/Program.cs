// See https://aka.ms/new-console-template for more information
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using DayMemory.Core.Settings;
using DayMemory.DAL;
using DayMemory.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Internal;
using System.IO;
using File = DayMemory.Core.Models.Personal.File;
using DayMemory.Core.Models.Common;
using SkiaSharp;
using static System.Net.WebRequestMethods;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

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

Console.WriteLine("End");



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