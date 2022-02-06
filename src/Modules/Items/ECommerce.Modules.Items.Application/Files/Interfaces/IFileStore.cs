using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Files.Interfaces
{
    public interface IFileStore
    {
        Task<IEnumerable<string>> WriteFilesAsync(ICollection<IFormFile> files, string path);
        Task<string> WriteFileAsync(IFormFile file, string path);
        Task<byte[]> ReadFileAsync(string path);
        string GetFileExtenstion(string file);
        string ReplaceInvalidChars(string fileName);
    }
}
