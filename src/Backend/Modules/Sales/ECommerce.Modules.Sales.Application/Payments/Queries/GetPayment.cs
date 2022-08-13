using ECommerce.Modules.Sales.Application.Payments.DTO;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Sales.Application.Payments.Queries
{
    public record GetPayment(Guid PaymentId) : IQuery<PaymentDto>;
}
