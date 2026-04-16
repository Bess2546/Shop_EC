using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Backend.UploadService
{
    public interface IUploadService
    {
        Task<string> UploadImageAsync(IFormFile file, string folder);
    }
}