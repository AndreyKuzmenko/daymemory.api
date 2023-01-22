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

var builder = new ConfigurationBuilder()
 .AddJsonFile($"appsettings.Development.json", true, true);
var config = builder.Build();


var storageConnectionString = config["AppUrls:StorageConnectionString"];
var connectionString = config["ConnectionStrings:DefaultConnection"];
var destContainerName = config["FileStorage:Container"];

var optionsBuilder = new DbContextOptionsBuilder<DayMemoryDbContext>();
optionsBuilder.UseSqlServer(connectionString);
var dbContext = new DayMemoryDbContext(optionsBuilder.Options);

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

Console.WriteLine("End");

