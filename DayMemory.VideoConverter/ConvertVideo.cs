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
using Microsoft.Extensions.Configuration;
using System.IO.Pipes;

namespace DayMemory.VideoConverter
{
    public class VideoParams
    {
        public string ConverterContainerName { get; set; }

        public string ContainerName { get; set; }

        public string UserId { get; set; }

        public string FileId { get; set; }

        public string InputFileName { get; set; }

        public string OutputFileName { get; set; }
    }

    public static class ConvertVideo
    {
        [FunctionName("ConvertVideo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {

            var content = await req.ReadAsStringAsync();
            var inputParams = JsonConvert.DeserializeObject<VideoParams>(content);


            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var storageConnectionString = config["Values:AzureWebJobsStorage"];

            if (string.IsNullOrEmpty(storageConnectionString))
            {
                storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            }

            log.LogInformation($"Video processing has been started for: '{inputParams.FileId}'");


            string tempPath = Path.GetTempPath();
            var exeFolder = Path.Combine(tempPath, "ffmpeg");
            var inputFileFolder = Path.Combine(tempPath, "ffmpeg", inputParams.FileId);
            var outputFileFolder = Path.Combine(tempPath, "ffmpeg", inputParams.FileId);

            if (!Directory.Exists(inputFileFolder))
            {
                Directory.CreateDirectory(inputFileFolder);
            }

            if (!Directory.Exists(exeFolder))
            {
                Directory.CreateDirectory(exeFolder);
            }

            log.LogInformation($"Loading converter");
            FFmpeg.SetExecutablesPath(exeFolder);
            //await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, exeFolder);

            var converterContainerClient = new BlobContainerClient(storageConnectionString, inputParams.ConverterContainerName);
            BlobClient mpegBlob = converterContainerClient.GetBlobClient($"ffmpeg.exe");
            BlobClient probeBlob = converterContainerClient.GetBlobClient($"ffprobe.exe");

            var ffmpegExeFilePath = Path.Combine(exeFolder, $"ffmpeg.exe");

            if (!System.IO.File.Exists(ffmpegExeFilePath))
            {
                using (var fileStream = System.IO.File.Create(ffmpegExeFilePath))
                {
                    log.LogInformation($"Downloading ffmpeg.exe");
                    var b = await mpegBlob.DownloadToAsync(fileStream);
                    if (b.IsError)
                    {
                        throw new InvalidOperationException("Can't download ffmpeg.exe");
                    }
                }
            }

            var ffprobeExeFilePath = Path.Combine(exeFolder, $"ffprobe.exe");
            if (!System.IO.File.Exists(ffprobeExeFilePath))
            {
                using (var fileStream = System.IO.File.Create(ffprobeExeFilePath))
                {
                    log.LogInformation($"Downloading ffprobe.exe");
                    var b = await probeBlob.DownloadToAsync(fileStream);
                    if (b.IsError)
                    {
                        throw new InvalidOperationException("Can't download ffprobe.exe");
                    }
                }
            }

            log.LogInformation($"Converter loaded");

            var inputFilePath = Path.Combine(inputFileFolder, $"{inputParams.InputFileName}");
            if (File.Exists(inputFilePath))
            {
                File.Delete(inputFilePath);
            }

            var outputFilePath = Path.Combine(outputFileFolder, $"{inputParams.OutputFileName}.mp4");
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            var blobContainerClient = new BlobContainerClient(storageConnectionString, inputParams.ContainerName);



            BlobClient inputBlob = blobContainerClient.GetBlobClient($"{inputParams.UserId}/{inputParams.FileId}/{inputParams.InputFileName}");
            BlobClient outputBlob = blobContainerClient.GetBlobClient($"{inputParams.UserId}/{inputParams.FileId}/{inputParams.OutputFileName}");
            using (var fileStream = System.IO.File.Create(inputFilePath))
            {
                log.LogInformation($"Loading input file");
                var b = await inputBlob.DownloadToAsync(fileStream);
                if (b.IsError)
                {
                    throw new InvalidOperationException("Can't download original video");
                }
            }

            log.LogInformation($"File convertion started");
            //var conversion = await FFmpeg.Conversions.FromSnippet.ToMp4(inputFilePath, outputFilePath);
            var conversion = await FFmpeg.Conversions.FromSnippet.ChangeSize(inputFilePath, outputFilePath, VideoSize.Hd720);

            await conversion.Start();
            log.LogInformation($"File convertion finished");

            using (var fileStream = System.IO.File.OpenRead(outputFilePath))
            {
                await outputBlob.DeleteIfExistsAsync();
                await outputBlob.UploadAsync(fileStream, new BlobUploadOptions() { HttpHeaders = new BlobHttpHeaders() { ContentType = "video/mp4" } });
            }
            log.LogInformation($"Converted File uploaded to storage");

            log.LogInformation($"Video processing has been finished for: '{inputParams.FileId}'");
            return new OkObjectResult("ok");
        }
    }
}
