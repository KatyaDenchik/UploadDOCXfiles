using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadDOCXfiles.Interfaces;

namespace UploadDOCXfiles.Validators
{
    public class DocxFilesValidator : IValidator
    {
        public bool Validate(string fileExtension)
        {
            return string.Equals(fileExtension, ".docx", StringComparison.OrdinalIgnoreCase);
        }
    }
}
