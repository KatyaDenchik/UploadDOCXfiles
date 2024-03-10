using Azure.Storage;
using Azure.Storage.Sas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Abstract
{
    public interface IBlobSasBuilder
    {
        public void SetPermissions(BlobSasPermissions permissions);
        public string ToSasQueryParameters(StorageSharedKeyCredential sharedKeyCredential);
    }
}
