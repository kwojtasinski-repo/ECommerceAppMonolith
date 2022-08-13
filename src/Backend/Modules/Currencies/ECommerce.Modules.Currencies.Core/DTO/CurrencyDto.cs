using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.DTO
{
    internal class CurrencyDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Code { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Description { get; set; }
    }
}
