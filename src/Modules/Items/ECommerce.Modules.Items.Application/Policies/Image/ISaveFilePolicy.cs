using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Policies.Image
{
    internal interface ISaveFilePolicy
    {
        public string GetFileDirectory();
        int GetAllowedSize();
        IEnumerable<string> GetAllowedImagesExtensions();
    }
}
