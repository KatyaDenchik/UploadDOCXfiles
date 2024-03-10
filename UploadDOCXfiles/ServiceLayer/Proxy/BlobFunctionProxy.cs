using Azure.Storage;
using Azure.Storage.Sas;
using ServiceLayer.Services;
using ServiceLayer.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Proxy
{
    public class BlobFunctionProxy : IBlobSasBuilder
    {
        private readonly BlobSasBuilder blobSasBuilder;
        public BlobFunctionProxy(BlobSasBuilder blobSasBuilder) 
        {
            this.blobSasBuilder = blobSasBuilder;
        }
        public void SetPermissions(BlobSasPermissions permissions)
        {
            blobSasBuilder.SetPermissions (permissions);
        }

        public string ToSasQueryParameters(StorageSharedKeyCredential sharedKeyCredential)
        {
           return blobSasBuilder.ToSasQueryParameters(sharedKeyCredential).ToString();
        }
    }
}
