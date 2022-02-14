using ECommerce.Modules.Items.Infrastructure.Files.Exceptions;
using ECommerce.Modules.Items.Application.Files.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Modules.Items.Infrastructure.Files.Implementations
{
    internal class FileStore : IFileStore
    {
        private readonly IFileWrapper _fileWrapper;
        private readonly IDirectoryWrapper _directoryWrapper;

        public FileStore(IFileWrapper fileWrapper, IDirectoryWrapper directoryWrapper)
        {
            _fileWrapper = fileWrapper;
            _directoryWrapper = directoryWrapper;
        }

        public string GetFileExtension(string file)
        {
            string ext = Path.GetExtension(file);
            return ext;
        }

        public async Task<byte[]> ReadFileAsync(string path)
        {
            var bytes = await _fileWrapper.ReadFileAsync(path);
            return bytes;
        }

        public string ReplaceInvalidChars(string fileName)
        {
            return string.Join("", fileName.Split(Path.GetInvalidFileNameChars()));
        }

        public async Task<string> WriteFileAsync(IFormFile file, string path)
        {
            _directoryWrapper.CreateDirectory(path);
            if (file != null)
            {
                string ext = GetFileExtension(file.FileName);
                var fileName = ReplaceInvalidChars(file.FileName);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName + ext;
                var outputFile = Path.Combine(path, uniqueFileName);
                var filePath = await _fileWrapper.WriteFileAsync(file, outputFile);
                return filePath;
            }
            else
            {
                throw new Exceptions.FileNotFoundException();
            }
        }

        public async Task<IEnumerable<string>> WriteFilesAsync(ICollection<IFormFile> files, string path)
        {
            _directoryWrapper.CreateDirectory(path);
            if (files != null && files.Count > 0)
            {
                var filesDirectory = new List<string>();
                foreach (IFormFile file in files)
                {
                    if (file != null)
                    {
                        string ext = Path.GetExtension(file.FileName);
                        var fileName = ReplaceInvalidChars(file.FileName);
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName + ext;
                        var outputFile = Path.Combine(path, uniqueFileName);
                        var filePath = await _fileWrapper.WriteFileAsync(file, outputFile);
                        filesDirectory.Add(filePath);
                    }
                }
                return filesDirectory;
            }
            else
            {
                throw new FilesNotFoundException();
            }
        }
    }
}
