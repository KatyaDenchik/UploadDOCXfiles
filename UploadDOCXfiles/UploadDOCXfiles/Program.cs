using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Radzen;
using System;
using System.IO;
using UploadDOCXfiles.Components;
using UploadDOCXfiles.Models;
using ServiceLayer.Services;
using ServiceLayer.Validators;
using ServiceLayer.Helpers;
using ServiceLayer.Services.Abstract;

namespace UploadDOCXfiles
{
    public class Program
    {
        public static async Task Main(string[] args)
        { 
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
            builder.Services.AddScoped<FormModel>();
            builder.Services.AddScoped<EmailValidator>();
            builder.Services.AddScoped<DocxFilesValidator>();
            builder.Services.AddScoped<NotificationService>();


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
