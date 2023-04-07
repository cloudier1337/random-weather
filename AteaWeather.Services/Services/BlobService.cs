using System.IO;
using System.Threading.Tasks;
using AteaWeather.Services.Services.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AteaWeather.Services.Services;

public class BlobService : IBlobService
{
    private static CloudBlobClient _client;
    private static string _containerName;

    public void Initialize(string connectionString)
    {
        var storageAccount = CloudStorageAccount.Parse(connectionString);
        _client = storageAccount.CreateCloudBlobClient();
        _containerName = "payloads";
    }

    public async Task<string> GetBlobAsync(string blobName)
    {
        var container = _client.GetContainerReference(_containerName);
        var blob = container.GetBlockBlobReference(blobName);

        // Check if blob exists
        if (!await blob.ExistsAsync())
        {
            throw new FileNotFoundException($"Blob {blobName} does not exist in container {_containerName}");
        }

        // blob content

        var blobText = await blob.DownloadTextAsync();
        
        return blobText;
    }
    
    public async Task StoreToBlob(string blobName, string blobContent)
    { 
        var container = _client.GetContainerReference(_containerName);
        var blob = container.GetBlockBlobReference(blobName);

        await blob.UploadTextAsync(blobContent);
    }
}