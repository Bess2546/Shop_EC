using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Backend.Upload
{
    public class UploadException : Exception
    {
        public UploadException(string message) : base(message) {}
        public UploadException(string message, Exception inner) : base(message, inner) {}
        
    }
}