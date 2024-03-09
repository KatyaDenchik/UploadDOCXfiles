using Azure.Storage.Blobs;

namespace ServiceLayer.Services.Abstract
{
    public interface IBlobStorageService
    {
        Task AddBlobMetadataAsync(BlobClient blob, string email);
        Task UploadAsync(MemoryStream memoryStream, string email, string fileName);
    }
}