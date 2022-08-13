using ECommerce.Modules.Items.Application.Files.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Modules.Items.Infrastructure.Files.Implementations
{
    internal class FileWrapper : IFileWrapper
    {
        public async Task<byte[]> ReadFileAsync(string path)
        {
            var bytes = await File.ReadAllBytesAsync(path);
            return bytes;
        }

        public void WriteAllBytes(string outputFile, byte[] content)
        {
            File.WriteAllBytes(outputFile, content);
        }

        public async Task<string> WriteFileAsync(IFormFile file, string outputFile)
        {
            try
            {
                using (FileStream fileStream = File.Create(outputFile))
                {
                    await file.CopyToAsync(fileStream);
                    fileStream.Flush();
                }
                return outputFile;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
