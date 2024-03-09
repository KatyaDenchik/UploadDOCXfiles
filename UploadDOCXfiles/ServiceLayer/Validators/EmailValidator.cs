using ServiceLayer.Validators.Abstract;
using System.Text.RegularExpressions;

namespace ServiceLayer.Validators
{
    public class EmailValidator : IValidator
    {
        /// <summary>
        /// Validates whether the specified string represents a valid email address using a regular expression pattern.
        /// </summary>
        /// <param name="email">The string to validate as an email address.</param>
        /// <returns>True if the specified string is a valid email address; otherwise, false.</returns>
        public bool Validate(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
