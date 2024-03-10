using ServiceLayer.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class EmailValidatorTests
    {
        [Test]
        public void Validate_WithValidEmail_ShouldReturnTrue()
        {
            // Arrange
            var validator = new EmailValidator();
            var validEmail = "test@gmail.com";

            // Act
            var result = validator.Validate(validEmail);

            // Assert
            Assert.IsTrue(result);
        }

        [TestCase("testgmail.com")]
        [TestCase("test@gmailcom")]
        [TestCase("12435")]
        public void Validate_WithInvalidEmail_ShouldReturnFalse(string email)
        {
            // Arrange
            var validator = new EmailValidator();
            var invalidEmail = email;

            // Act
            var result = validator.Validate(invalidEmail);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
