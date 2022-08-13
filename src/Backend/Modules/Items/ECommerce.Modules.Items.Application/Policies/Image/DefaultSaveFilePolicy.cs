using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Policies.Image
{
    internal class DefaultSaveFilePolicy : ISaveFilePolicy
    {
        private readonly static string FILE_DIR = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Upload" + Path.DirectorySeparatorChar + "Files" + Path.DirectorySeparatorChar + Guid.NewGuid().ToString();
        private readonly int ALLOWED_SIZE = 10 * 1024 * 1024; // 10 mb
        private readonly List<string> IMAGE_EXTENSION_PARAMETERS = new List<string> { ".jpg", ".png" }; // extensions

        public string GetFileDirectory()
        {
            return FILE_DIR;
        }

        public int GetAllowedSize()
        {
            return ALLOWED_SIZE;
        }

        public IEnumerable<string> GetAllowedImagesExtensions()
        {
            return IMAGE_EXTENSION_PARAMETERS;
        }
    }
}
