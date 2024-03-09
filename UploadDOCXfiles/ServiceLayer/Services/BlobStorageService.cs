using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceLayer.Helpers;
using ServiceLayer.Services.Abstract;

namespace ServiceLayer.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        public const string ContainerName = "docxfiles";
        private readonly string connectionString;
        private readonly ILogger logger;
        private readonly BlobServiceClient blobServiceClient;

        private BlobContainerClient containerClient;

        public BlobStorageService(ILoggerFactory loggerFactory)
        {
            connectionString = SecretsHelper.GetSecret<BlobStorageService>("Blob", "BlobConnectionString");
            logger = loggerFactory.CreateLogger<BlobStorageService>();
            blobServiceClient = new BlobServiceClient(connectionString);
            InitContainer();
        }


        /// <summary>
        /// Uploads the content of the provided memory stream to the blob storage container with the specified metadata.
        /// </summary>
        /// <param name="memoryStream">The memory stream containing the content to be uploaded.</param>
        /// <param name="email">The email metadata value to be associated with the uploaded blob.</param>
        /// <param name="fileName">The name of the file to be used as the blob's name.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task UploadAsync(MemoryStream memoryStream, string email, string fileName)
        {
            var blobClient = containerClient.GetBlobClient(fileName);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream, true);
            await AddBlobMetadataAsync(blobClient, email);
        }

        /// <summary>
        /// Adds metadata to the specified blob.
        /// </summary>
        /// <param name="blob">The BlobClient representing the blob.</param>
        /// <param name="email">The email metadata value to be added.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task AddBlobMetadataAsync(BlobClient blob, string email)
        {
            try
            {
                IDictionary<string, string> metadata =
                   new Dictionary<string, string>();

                metadata["email"] = email;

                await blob.SetMetadataAsync(metadata);
            }
            catch (RequestFailedException e)
            {
                logger.LogError(e.ToString());
            }
        }

        /// <summary>
        /// Initializes the blob container with the specified name.
        /// </summary>
        private async void InitContainer()
        {
            var client = blobServiceClient.GetBlobContainers().FirstOrDefault(s => s.Name.Equals(ContainerName));
            if (client is not null)
            {
                containerClient = blobServiceClient.GetBlobContainerClient(client.Name);
                return;
            }
            containerClient = await blobServiceClient.CreateBlobContainerAsync(ContainerName);
        }

    }
}
