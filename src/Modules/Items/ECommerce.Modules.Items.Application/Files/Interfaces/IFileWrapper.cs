using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Files.Interfaces
{
    public interface IFileWrapper
    {
        void WriteAllBytes(string outputFile, byte[] content);
        Task<string> WriteFileAsync(IFormFile file, string outputFile);
        Task<byte[]> ReadFileAsync(string path);
    }
}
