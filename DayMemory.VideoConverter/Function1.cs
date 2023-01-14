using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mime;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace DayMemory.VideoConverter
{
    public static class ConvertVideo
    {
        [FunctionName("ConvertVideo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string tempPath = Path.GetTempPath();
            var exeFolder = Path.Combine(tempPath, "ffmpeg");
            var inputFileFolder = Path.Combine(tempPath, "ffmpeg", "111111111");
            var outputFileFolder = Path.Combine(tempPath, "ffmpeg", "111111111");

            if (!Directory.Exists(inputFileFolder))
            {
                Directory.CreateDirectory(inputFileFolder);
            }

            if (!Directory.Exists(exeFolder))
            {
                Directory.CreateDirectory(exeFolder);
            }

            FFmpeg.SetExecutablesPath(exeFolder);
            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, exeFolder);

            

            //var containerName = "note-images";
            //var blobContainerClient = new BlobContainerClient("", containerName);

            //BlobClient blob = blobContainerClient.GetBlobClient("a77f5313-1824-4b4d-ad54-b8b9f39af3e1/111111111");
            //using (var stream = new MemoryStream())
            //{
            //    var b = await blob.DownloadToAsync(stream);
            //    if (!b.IsError)
            //    {

            //    }
            //}

            //await blob.UploadAsync(stream, new BlobUploadOptions() { HttpHeaders = new BlobHttpHeaders() { ContentType = contentType } });


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
