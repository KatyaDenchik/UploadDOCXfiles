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
        public bool Validate(string file)
        {
            if (File.Exists(file))
            {
                string extension = Path.GetExtension(file);
                return extension.Equals(".docx", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
