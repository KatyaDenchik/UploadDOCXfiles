using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using UploadDOCXfiles.Components;

namespace UploadDOCXfiles
{
    public class Program
    {
        public static async Task Main(string[] args)
        { 
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=docxfilestoragecreate;AccountKey=j1p5SuPFEzerGiO/9IZhSvC6/Tz/C1WKoHmiMlL9NR5HwBjdRFqFRIeH39ywLl2bzKn/2CQBtXGo+AStJqkoKQ==;EndpointSuffix=core.windows.net";

            var blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = "docxfiles" + Guid.NewGuid().ToString();

            // Create the container and return a container client object
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
