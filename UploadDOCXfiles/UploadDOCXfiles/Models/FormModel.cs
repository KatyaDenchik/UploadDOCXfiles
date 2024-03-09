using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Services;
using ServiceLayer.Validators;
using static ServiceLayer.Extensions.EnumExtensions;

namespace UploadDOCXfiles.Models
{
    public class FormModel
    {
        private readonly BlobStorageService blobStorageService;
        private readonly EmailValidator emailValidator;
        private readonly DocxFilesValidator docxFilesValidator;
        private readonly NotificationService notificationService;
        public FormModel(BlobStorageService blobStorageService, EmailValidator emailValidator,
                         DocxFilesValidator docxFilesValidator, NotificationService notificationService)
        {
            this.blobStorageService = blobStorageService;
            this.emailValidator = emailValidator;
            this.docxFilesValidator = docxFilesValidator;
            this.notificationService = notificationService;
        }
        private string email;
        public string Email
        {
            get => email;
            set
            {
                if (emailValidator.Validate(value))
                {
                    email = value;
                }
                else email = string.Empty;
            }
        }
        public MemoryStream File { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string InputFileId { get; private set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Handles the event raised when a file is selected.
        /// </summary>
        /// <param name="e">The event arguments containing information about the selected file.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;

            if (file != null)
            {

                FileName = file.Name;

                FileExtension = Path.GetExtension(FileName);

                File = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(File);
            }
        }

        /// <summary>
        /// Uploads the selected file to the blob storage service if it is valid, and displays a success or error notification accordingly.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task UploadFile()
        {
            if (File != null && !string.IsNullOrEmpty(FileExtension) && docxFilesValidator.Validate(FileExtension))
            {
                await blobStorageService.UploadAsync(File, Email, FileName);
                ShowNotification("Success", "Файл успішно відправлено");
                ClearFields();
            }
            else
            {
                ShowNotification("Error", "Не вдалося відправити файл");
            }
        }

        /// <summary>
        /// Clears the input fields by resetting the email, file extension, and input file ID properties.
        /// </summary>
        private void ClearFields()
        {
            Email = string.Empty;
            FileExtension = string.Empty;
            InputFileId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Displays a notification with the specified severity and message.
        /// </summary>
        /// <param name="severity">The severity level of the notification.</param>
        /// <param name="message">The message content of the notification.</param>
        private void ShowNotification(string severity, string message)
        {
            notificationService.Notify(severity.ToEnum<NotificationSeverity>(), message);
        }
    }
}
