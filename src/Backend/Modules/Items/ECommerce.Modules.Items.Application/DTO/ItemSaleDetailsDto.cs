﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.DTO
{
    public class ItemSaleDetailsDto
    {
        public Guid Id { get; set; }
        public ItemDetailsDto Item { get; set; }
        public decimal Cost { get; set; }
        public string CurrencyCode { get; set; }
        public bool Active { get; set; }
    }
}
