using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Clients.External
{
    internal class NbpClientOptions
    {
        public string BaseUrl { get; set; }
        public int Timeout { get; set; }
    }
}
