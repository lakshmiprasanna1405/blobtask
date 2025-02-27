using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;

public class Function1
{
    private readonly ILogger _logger;

    public Function1(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Function1>();
    }

    [Function("Function1")]
    public async Task Run([TimerTrigger("*/2 * * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        string connectionString = Environment.GetEnvironmentVariable("BlobStorageConnectionString");
        string containerName = "blobtask";
        string blobName = $"blob-{DateTime.Now:yyyy-MM-dd-HH-mm-ss-fff}.txt";
        string content = "Lakshmi!";

        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        using (MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content)))
        {
            await blobClient.UploadAsync(ms, true);
        }

        _logger.LogInformation($"Blob uploaded: {blobName}");
    }
}
