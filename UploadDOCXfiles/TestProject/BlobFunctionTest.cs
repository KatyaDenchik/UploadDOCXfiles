using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlobFunction;
using Microsoft.Extensions.Logging;
using ServiceLayer.Services.Abstract;
using Moq;
using Radzen;
using ServiceLayer.Validators;
using TestProject.CustomMocks;
using ServiceLayer.SubModels;

namespace TestProject
{
    public class BlobFunctionTest
    {
        [Test]
        public void GetSasToken_ReturnsValidToken()
        {
            // Arrange
            string testToken = "testToken";

            var loggerMock = new Mock<ILoggerFactory>();
            var emailServicesMock = new Mock<IEmailServices>();
            var blobStorageServicMock = new Mock<IBlobStorageService>();
            var blobSasBuilderMock = new BlobSasBuilderMock(testToken);

            var blobTriggerFunction = new BlobTriggerFunction(loggerMock.Object, emailServicesMock.Object, blobStorageServicMock.Object);

            // Act
            string token = blobTriggerFunction.GetSasToken(blobSasBuilderMock, null);

            // Assert
            Assert.AreEqual(testToken, token);
        }

        [Test]
        public void Run_ValidInput_SendEmail()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();

            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
            var emailServicesMock = new Mock<IEmailServices>();
            var blobStorageServiceMock = new Mock<IBlobStorageService>();

            var metaData = new Dictionary<string, string>
            {
                { "somekey", "somevalue" }
            };
            var uri = new Uri("http://example.com");
            var name = "test.docx";
            var blob = "blobcontent";

            var classUnderTest = new BlobTriggerFunction(loggerFactoryMock.Object, emailServicesMock.Object, blobStorageServiceMock.Object);
            blobStorageServiceMock.Setup(x => x.GetBlobMetadataEmailAsync(It.IsAny<IDictionary<string, string>>())).ReturnsAsync("test@example.com");

            // Act
            classUnderTest.Run(blob, name, uri, metaData);

            // Assert
            emailServicesMock.Verify(x => x.SendEmail(It.IsAny<EmailInformation>()), Times.Once);
        }
    }
}
