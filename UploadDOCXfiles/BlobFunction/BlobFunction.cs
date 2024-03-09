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

namespace BlobFunction
{
    /// <summary>
    /// Blob Function that reacts to adding an object to the container 'docxfiles'
    /// </summary>
    public class BlobFunction
    {
        private readonly ILogger logger;
        private readonly IEmailServices emailServices;

        public BlobFunction(ILoggerFactory loggerFactory, IEmailServices emailServices)
        {
            logger = loggerFactory.CreateLogger<BlobFunction>();
            this.emailServices = emailServices;
        }
        /// <summary>
        /// Method that will be called when adding a new object to the container 'docxfiles'
        /// </summary>
        /// <param name="blob">String representation of the object that was added</param>
        /// <param name="name">Name of the object that was added</param>
        /// <param name="uri">Uri of the object that was added</param>
        /// <param name="metaData">Metadata of the object that was added</param>
        [Function(nameof(BlobFunction))]
        public void Run([BlobTrigger("docxfiles/{name}", Connection = "AzureWebJobsStorage")] string blob, string name, Uri uri, IDictionary<string, string> metaData)
        {
            var address = GetBlobMetadataEmailAsync(metaData).Result;

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
        private string GetSasToken(string name)
        {
            var builder = new BlobSasBuilder()
            {
                BlobContainerName = BlobStorageService.ContainerName,
                BlobName = name,
                ExpiresOn = DateTime.UtcNow.AddHours(1),
            };
            builder.SetPermissions(BlobSasPermissions.All);
            string accountName = "docxfilestoragecreate";
            string accountKey = Environment.GetEnvironmentVariable("AccountKey");
            var token = builder.ToSasQueryParameters(new StorageSharedKeyCredential(accountName, accountKey)).ToString();
            return token;
        }

        /// <summary>
        /// Asynchronously retrieves the value associated with the 'email' key from the specified metadata dictionary.
        /// </summary>
        /// <param name="metadata">The dictionary containing metadata.</param>
        /// <returns>
        /// The value associated with the 'email' key if found; otherwise, an empty string.
        /// </returns>
        private async Task<string> GetBlobMetadataEmailAsync(IDictionary<string, string> metadata)
        {
            try
            {
                if (metadata.ContainsKey("email"))
                {
                    return metadata["email"];
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
            }
            logger.LogError("Key 'email' not found in metadata");
            return string.Empty;
        }
    }
}
