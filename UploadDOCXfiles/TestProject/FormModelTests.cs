using Microsoft.AspNetCore.Components.Forms;
using UploadDOCXfiles.Models;
using ServiceLayer.Services;
using ServiceLayer.Validators;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using Radzen;
using ServiceLayer.Validators;

namespace TestProject
{
    public class FormModelTests
    {
        private BlobStorageService mockBlobStorageService;
        private EmailValidator mockEmailValidator;
        private DocxFilesValidator mockDocxFilesValidator;
        private NotificationService mockNotificationService;

        [SetUp]
        public void Setup()
        {
            //mockBlobStorageService = new BlobStorageService();
            //mockEmailValidator = new EmailValidator();
            //mockDocxFilesValidator = new DocxFilesValidator(); 
            //mockNotificationService = new NotificationService();
        }

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

        //[Test]
        //public async Task HandleFileSelected_SetsFileAndFileExtension_WhenInputFileChangeEventArgsIsNotNull()
        //{
        //    // Arrange
        //    var formModel = new FormModel(mockBlobStorageService, mockEmailValidator, mockDocxFilesValidator);
        //    var mockInputFileChangeEventArgs = new InputFileChangeEventArgs();
        //    var mockFileListEntry = new Mock<IFileListEntry>();
        //    mockFileListEntry.Setup(entry => entry.Name).Returns("test.docx");
        //    mockInputFileChangeEventArgs.File = mockFileListEntry.Object;

        //    // Act
        //    await formModel.HandleFileSelected(mockInputFileChangeEventArgs);

        //    // Assert
        //    Assert.NotNull(formModel.File);
        //    Assert.NotNull(formModel.FileExtension);
        //}

        [Test]
        public async Task HandleFileSelected_DoesNotSetFileAndFileExtension_WhenInputFileChangeEventArgsIsNull()
        {
            // Arrange
            var formModel = new FormModel(mockBlobStorageService, mockEmailValidator, mockDocxFilesValidator, mockNotificationService);
            InputFileChangeEventArgs mockInputFileChangeEventArgs = null;

            // Act
            await formModel.HandleFileSelected(mockInputFileChangeEventArgs);

            // Assert
            Assert.Null(formModel.File);
            Assert.Null(formModel.FileExtension);
        }
    }
}