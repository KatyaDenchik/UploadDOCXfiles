using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadDOCXfiles.Services;

namespace UploadDOCXfiles.Models
{
    public class FormModel
    {
        private readonly BlobStorageService blobStorageService;
        public FormModel(BlobStorageService blobStorageService)
        {
            this.blobStorageService = blobStorageService;
        }
        public string Email { get; set; }
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
            if (File != null)
            {
                await blobStorageService.UploadAsync(File);
            }
        }
    }
}
