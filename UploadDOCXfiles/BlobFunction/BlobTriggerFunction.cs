using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BlobFunction
{
    public class BlobTriggerFunction
    {
        [FunctionName("BlobTriggerFunction")]
        public void Run([BlobTrigger("docxfiles/{name}", Connection = "DefaultEndpointsProtocol=https;AccountName=docxfilestoragecreate;AccountKey=j1p5SuPFEzerGiO/9IZhSvC6/Tz/C1WKoHmiMlL9NR5HwBjdRFqFRIeH39ywLl2bzKn/2CQBtXGo+AStJqkoKQ==;EndpointSuffix=core.windows.net")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
