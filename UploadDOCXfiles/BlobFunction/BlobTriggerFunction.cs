using System;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using Azure.Storage.Sas;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure;
using ServiceLayer.Services.Abstract;
using ServiceLayer.SubModels;
using ServiceLayer.Services;
using ServiceLayer.Proxy;

namespace BlobFunction
{
    /// <summary>
    /// Blob Function that reacts to adding an object to the container 'docxfiles'
    /// </summary>
    public class BlobTriggerFunction
    {
        private readonly ILogger logger;
        private readonly IEmailServices emailServices;
        private readonly IBlobStorageService blobStorageService;

        public BlobTriggerFunction(ILoggerFactory loggerFactory, IEmailServices emailServices, IBlobStorageService blobStorageService)
        {
            logger = loggerFactory.CreateLogger<BlobTriggerFunction>();
            this.emailServices = emailServices;
            this.blobStorageService = blobStorageService;
        }
        /// <summary>
        /// Method that will be called when adding a new object to the container 'docxfiles'
        /// </summary>
        /// <param name="blob">String representation of the object that was added</param>
        /// <param name="name">Name of the object that was added</param>
        /// <param name="uri">Uri of the object that was added</param>
        /// <param name="metaData">Metadata of the object that was added</param>
        [Function(nameof(BlobTriggerFunction))]
        public void Run([BlobTrigger("docxfiles/{name}", Connection = "AzureWebJobsStorage")] string blob, string name, Uri uri, IDictionary<string, string> metaData)
        {
            var address = blobStorageService.GetBlobMetadataEmailAsync(metaData).Result;

            if (string.IsNullOrEmpty(address))
            {
                logger.LogError("Invalid address. Email didn`t sent");
                return;
            }

            var sasToken = GetSasToken(name); 
            var sasUrl = uri.AbsoluteUri + "?" + sasToken;
            emailServices.SendEmail(new EmailInformation { Address = address, Body = $"<p>Посилання на ваш файл <a href=\"{sasUrl}\">{name}</a></p>", From = "Kateryna Denchyk Test Task", To = "Reenbit Team" });
        }

        /// <summary>
        /// Method for generating Sas token
        /// </summary>
        /// <param name="name">Name of the object for generate the Sas token</param>
        /// <returns>Sas token</returns>
        public string GetSasToken(string name)
        {
            var builder = new BlobSasBuilder()
            {
                BlobContainerName = BlobStorageService.ContainerName,
                BlobName = name,
                ExpiresOn = DateTime.UtcNow.AddHours(1),
            };
            string accountName = "docxfilestoragecreate";
            string accountKey = Environment.GetEnvironmentVariable("AccountKey");

            if (string.IsNullOrEmpty(accountKey))
            {
                logger.LogError("AccountKey is empty");
                return string.Empty;
            }
            var credential = new StorageSharedKeyCredential(accountName, accountKey);
            return GetSasToken(new BlobFunctionProxy(builder), credential);
        }

        public string GetSasToken(IBlobSasBuilder blobSasBuilder, StorageSharedKeyCredential credential)
        {
            blobSasBuilder.SetPermissions(BlobSasPermissions.All);
            var token = blobSasBuilder.ToSasQueryParameters(credential);
            return token;
        }
    }
}
