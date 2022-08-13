using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Abstractions.Time
{
    public interface IClock
    {
        DateTime CurrentDate();
    }
}
