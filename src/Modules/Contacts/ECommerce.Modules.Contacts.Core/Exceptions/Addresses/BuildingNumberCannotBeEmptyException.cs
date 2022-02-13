using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class BuildingNumberCannotBeEmptyException : ECommerceException
    {
        public BuildingNumberCannotBeEmptyException() : base("BuildingNumber cannot be empty.")
        {
        }
    }
}
