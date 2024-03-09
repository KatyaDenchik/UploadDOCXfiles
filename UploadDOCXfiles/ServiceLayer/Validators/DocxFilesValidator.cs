using ServiceLayer.Validators.Abstract;

namespace ServiceLayer.Validators
{
    public class DocxFilesValidator : IValidator
    {
        /// <summary>
        /// Validates whether the specified file extension is ".docx" (case-insensitive).
        /// </summary>
        /// <param name="fileExtension">The file extension to validate.</param>
        /// <returns>True if the file extension is ".docx"; otherwise, false.</returns>
        public bool Validate(string fileExtension)
        {
            return string.Equals(fileExtension, ".docx", StringComparison.OrdinalIgnoreCase);
        }
    }
}
