using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;

namespace ServiceLayer.Services.Abstract
{
    public interface IBlobStorageService
    {
        Task AddBlobMetadataAsync(BlobBaseClient blob, string email);
        Task UploadAsync(MemoryStream memoryStream, string email, string fileName);
        Task<string> GetBlobMetadataEmailAsync(IDictionary<string, string> metadata);
    }
}