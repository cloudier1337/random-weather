using System.Threading.Tasks;

namespace AteaWeather.Services.Services.Interfaces;

public interface IBlobService
{
    void Initialize();
    Task<string> GetBlobAsync(string blobName);
    Task StoreToBlob(string blobName, string blobContent);
}