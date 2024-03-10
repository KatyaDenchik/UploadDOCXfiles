using Microsoft.AspNetCore.Components.Forms;
using UploadDOCXfiles.Models;
using ServiceLayer.Services;
using ServiceLayer.Validators;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using Radzen;
using ServiceLayer.Validators;
using Microsoft.AspNetCore.Http;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading;
using ServiceLayer.Services.Abstract;

namespace TestProject
{
    public class FormModelTests
    {
        private BlobStorageService mockBlobStorageService;
        private EmailValidator mockEmailValidator;
        private DocxFilesValidator mockDocxFilesValidator;
        private NotificationService mockNotificationService;
        private ILoggerFactory mockLoggerFactory;


        [Test]
        public void EmailSetter_SetsEmail_WhenValidEmailIsPassed()
        {
            // Arrange
            var formModel = new FormModel(mockBlobStorageService, mockEmailValidator, mockDocxFilesValidator, mockNotificationService);
            var validEmail = "test@example.com";

            // Act
            formModel.Email = validEmail;

            // Assert
            Assert.AreEqual(validEmail, formModel.Email);
        }

        [Test]
        [TestCase("testexample.com")]
        [TestCase("1234234")]
        [TestCase("sadasfas")]
        [TestCase("testexample@com")]
        public void EmailSetter_SetsEmail_WhenInvalidEmailIsPassed(string invalidEmail)
        {
            // Arrange
            var formModel = new FormModel(mockBlobStorageService, mockEmailValidator, mockDocxFilesValidator, mockNotificationService);

            // Act
            formModel.Email = invalidEmail;

            // Assert
            Assert.AreNotEqual(invalidEmail, formModel.Email);
            Assert.IsEmpty(formModel.Email);
        }

        [Test]
        public async Task HandleFileSelected_WhenFileIsSelected_ShouldSetFileNameAndFileExtension()
        {
            // Arrange
            var mockBrowserFile = new Mock<IBrowserFile>();

            mockBrowserFile.Setup(x => x.OpenReadStream(512000, default)).Returns(() =>
            {
                return new MemoryStream();
            });
            mockBrowserFile.SetupGet(x => x.Name).Returns("file.docx");

            var formModel = new FormModel(mockBlobStorageService, mockEmailValidator, mockDocxFilesValidator, mockNotificationService);

            // Act
            await formModel.HandleFileSelected(new InputFileChangeEventArgs(new List<IBrowserFile>() { mockBrowserFile.Object }));

            // Assert
            Assert.AreEqual("file.docx", formModel.FileName);
            Assert.AreEqual(".docx", formModel.FileExtension);
        }
        [Test]
        public async Task HandleFileSelected_WhenFileIsSelected_ShouldNotSetWrongFileNameAndFileExtension()
        {
            // Arrange
            var mockBrowserFile = new Mock<IBrowserFile>();

            mockBrowserFile.Setup(x => x.OpenReadStream(512000, default)).Returns(() =>
            {
                return new MemoryStream();
            });
            mockBrowserFile.SetupGet(x => x.Name).Returns("file.txt");

            var formModel = new FormModel(mockBlobStorageService, mockEmailValidator, mockDocxFilesValidator, mockNotificationService);
            // Act
            await formModel.HandleFileSelected(new InputFileChangeEventArgs(new List<IBrowserFile>() { mockBrowserFile.Object }));

            // Assert
            Assert.AreNotEqual("file.txt", formModel.FileName);
            Assert.AreNotEqual(".txt", formModel.FileExtension);
        }

        [Test]
        public void UploadFileSuccess()
        {
            // Arrange
            var mockBlobStorageService = new Mock<IBlobStorageService>();
            mockBlobStorageService.Setup(x => x.UploadAsync(default, default, default));


            var formModel = new FormModel(mockBlobStorageService.Object, mockEmailValidator, mockDocxFilesValidator, mockNotificationService);
            formModel.File = new MemoryStream();
            formModel.FileExtension = ".docx";
            formModel.Email = "test@example.com";
            var firstId = formModel.InputFileId;
            // Act
            formModel.UploadFile();

            var secondId = formModel.InputFileId;

            // Assert
            Assert.IsEmpty(formModel.Email);
            Assert.IsEmpty(formModel.FileExtension);
            Assert.IsNull(formModel.File);
            Assert.AreNotEqual(firstId, secondId);
        }
        [Test]
        [TestCase(true, ".docx")]
        [TestCase(false, ".txt")]
        public void UploadFileUnsuccess(bool fileIsNull, string extension)
        {
            // Arrange
            var mockBlobStorageService = new Mock<IBlobStorageService>();
            mockBlobStorageService.Setup(x => x.UploadAsync(default, default, default));

            MemoryStream? file;
            if (fileIsNull)
            {
                file = null;
            }
            else
            {
                file = new MemoryStream();
            }

            var formModel = new FormModel(mockBlobStorageService.Object, mockEmailValidator, mockDocxFilesValidator, mockNotificationService);
            formModel.File = file;
            formModel.FileExtension = extension;
            formModel.Email = "test@example.com";
            var firstId = formModel.InputFileId;
            // Act
            formModel.UploadFile();

            var secondId = formModel.InputFileId;

            // Assert
            Assert.AreEqual(formModel.File, file);
            Assert.AreEqual(formModel.FileExtension, extension);
            Assert.AreEqual(firstId, secondId);
        }
    }
}