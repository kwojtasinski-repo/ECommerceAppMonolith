using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class BuildingNumberTooLongException : ECommerceException
    {
        public string BuildingNumber { get; set; }

        public BuildingNumberTooLongException(string buildingNumber) : base($"BuildingNumber '{buildingNumber}' is too long.")
        {
            BuildingNumber = buildingNumber;
        }
    }
}
