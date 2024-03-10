using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class BlobStorageServiceTest
    {
        [Test]
        public async Task AddBlobMetadataTest()
        {
            // Arrange
            var blobClientMock = new Mock<BlobBaseClient>(MockBehavior.Strict, new Uri("https://yourblob.blob.core.windows.net/container/blob.txt"), null);

            var email = "test@example.com";

            IDictionary<string, string> expectedMetadata = new Dictionary<string, string>
        {
            { "email", email }
        };

            blobClientMock.Setup(b => b.SetMetadataAsync(expectedMetadata, null, new CancellationToken()))
                          .Returns(Task.FromResult(It.IsAny<Response<BlobInfo>>()));

            var loggerMock = new Mock<ILoggerFactory>();

            var blobStorageService = new BlobStorageService(loggerMock.Object);

            // Act
            await blobStorageService.AddBlobMetadataAsync(blobClientMock.Object, email);

            // Assert
            blobClientMock.Verify(b => b.SetMetadataAsync(expectedMetadata, null, new CancellationToken()), Times.Once);
        }
        [Test]
        public async Task AddBlobMetadataAsync_ExceptionLogged()
        {
            // Arrange
            var blobClientMock = new Mock<BlobClient>(MockBehavior.Strict, new Uri("https://yourblob.blob.core.windows.net/container/blob.txt"), null);

            var email = "test@example.com";

            IDictionary<string, string> expectedMetadata = new Dictionary<string, string>
        {
            { "email", email }
        };

            var exception = new RequestFailedException("Test exception");

            blobClientMock.Setup(b => b.SetMetadataAsync(expectedMetadata, null, new CancellationToken()))
                          .ThrowsAsync(exception);

            var loggerMock = new Mock<ILogger>();

            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

            var blobStorageService = new BlobStorageService(loggerFactoryMock.Object);

            // Act
            await blobStorageService.AddBlobMetadataAsync(blobClientMock.Object, email);

            // Assert
            blobClientMock.Verify(b => b.SetMetadataAsync(expectedMetadata, null, new CancellationToken()), Times.Once);
            loggerMock.Verify(logger =>
            logger.Log(
                 It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                 It.Is<EventId>(eventId => eventId.Id == 0),
                 It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == exception.ToString() && @type.Name == "FormattedLogValues"),
                 It.IsAny<Exception>(),
                 It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                 Times.Once);
        } 

        [Test]
        public async Task GetBlobMetadataEmailSuccessTest()
        {
            var email = "test@example.com";

            IDictionary<string, string> expectedMetadata = new Dictionary<string, string>
        {
            { "email", email }
        };
            var loggerMock = new Mock<ILoggerFactory>();

            var blobStorageService = new BlobStorageService(loggerMock.Object);

            var value = await blobStorageService.GetBlobMetadataEmailAsync(expectedMetadata);

           Assert.AreEqual(email, value);
        }

        [Test]
        public async Task GetBlobMetadataEmailUnsuccessTest()
        {
            var email = "test@example.com";

            IDictionary<string, string> expectedMetadata = new Dictionary<string, string>
        {
            { "test", email }
        };
            var loggerMock = new Mock<ILogger>();

            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

            var blobStorageService = new BlobStorageService(loggerFactoryMock.Object);

            var value = await blobStorageService.GetBlobMetadataEmailAsync(expectedMetadata);

            Assert.IsEmpty(value);
        }
    }
}
