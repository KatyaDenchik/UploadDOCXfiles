using Azure.Storage;
using Azure.Storage.Sas;
using ServiceLayer.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.CustomMocks
{
    public class BlobSasBuilderMock : IBlobSasBuilder
    {
        private readonly string token;
        public BlobSasBuilderMock(string token)
        {
            this.token = token;   
        }
        public void SetPermissions(BlobSasPermissions permissions)
        {
        }

        public string ToSasQueryParameters(StorageSharedKeyCredential sharedKeyCredential)
        {
            return token;
        }
    }
}
