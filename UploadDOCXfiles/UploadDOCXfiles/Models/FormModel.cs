﻿using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadDOCXfiles.Services;
using UploadDOCXfiles.Validators;

namespace UploadDOCXfiles.Models
{
    public class FormModel
    {
        private readonly BlobStorageService blobStorageService;
        private readonly EmailValidator emailValidator;
        private readonly DocxFilesValidator docxFilesValidator;
        public FormModel(BlobStorageService blobStorageService, EmailValidator emailValidator, DocxFilesValidator docxFilesValidator)
        {
            this.blobStorageService = blobStorageService;
            this.emailValidator = emailValidator;
            this.docxFilesValidator = docxFilesValidator;
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
            }
        }
        public MemoryStream File { get; set; }

        public async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;

            if (file != null)
            {
                File = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(File);
            }
        }

        public async Task UploadFile()
        {
            if (File != null )
            {
                await blobStorageService.UploadAsync(File);
            }
        }
    }
}