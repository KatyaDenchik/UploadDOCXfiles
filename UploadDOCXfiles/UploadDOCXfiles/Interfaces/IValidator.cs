using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadDOCXfiles.Interfaces
{
    public interface IValidator
    {
        public bool Validate(string inputString);
    }
}
