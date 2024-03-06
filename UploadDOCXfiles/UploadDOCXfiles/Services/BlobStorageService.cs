using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadDOCXfiles.Services
{
    public class BlobStorageService
    {
        string containerName = "docxfiles";
        BlobContainerClient containerClient;
        BlobServiceClient blobServiceClient;
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=docxfilestoragecreate;AccountKey=j1p5SuPFEzerGiO/9IZhSvC6/Tz/C1WKoHmiMlL9NR5HwBjdRFqFRIeH39ywLl2bzKn/2CQBtXGo+AStJqkoKQ==;EndpointSuffix=core.windows.net";

        public BlobStorageService()
        {
            blobServiceClient = new BlobServiceClient(connectionString);
            InitContainer();
        }
        public async Task UploadAsync(MemoryStream memoryStream)
        {
            var fileName = Guid.NewGuid().ToString() + ".docx";

            var blobClient = containerClient.GetBlobClient(fileName);

            memoryStream.Position = 0;
            var t = await blobClient.UploadAsync(memoryStream, true);
        }

        private async void InitContainer()
        {
            var client = blobServiceClient.GetBlobContainers().FirstOrDefault(s => s.Name.Equals(containerName));
            if (client is not null)
            {
                containerClient = blobServiceClient.GetBlobContainerClient(client.Name);
                return;
            }
            containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
        }
    }
}
