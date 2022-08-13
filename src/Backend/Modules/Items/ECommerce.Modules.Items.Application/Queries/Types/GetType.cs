using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Shared.Abstractions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Queries.Types
{
    public record GetType(Guid TypeId) : IQuery<TypeDto>;
}
