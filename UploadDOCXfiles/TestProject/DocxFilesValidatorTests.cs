using NUnit.Framework;
using ServiceLayer.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class DocxFilesValidatorTests
    {
        [Test]
        public void Validate_WithValidFileExtension_ShouldReturnTrue()
        {
            // Arrange
            var validator = new DocxFilesValidator();
            var fileExtension = ".docx";

            // Act
            var result = validator.Validate(fileExtension);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Validate_WithInvalidFileExtension_ShouldReturnFalse()
        {
            // Arrange
            var validator = new DocxFilesValidator();
            var fileExtension = ".txt";

            // Act
            var result = validator.Validate(fileExtension);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
