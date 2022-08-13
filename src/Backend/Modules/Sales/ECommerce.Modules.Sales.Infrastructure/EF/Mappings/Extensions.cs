using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Modules.Sales.Application.Payments.DTO;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Payments.Entities;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Mappings
{
    public static class Extensions
    {
        public static OrderDto AsDto(this Order order)
        {
            var dto = new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Cost = order.Price.Value,
                CreateOrderDate = order.CreateOrderDate,
                OrderApprovedDate = order.OrderApprovedDate,
                CustomerId = order.CustomerId,
                Paid = order.Paid,
                UserId = order.UserId,
                Code = order.Currency.CurrencyCode,
                Rate = order.Currency.Rate
            };
            return dto;
        }

        public static OrderDetailsDto AsDetailsDto(this Order order)
        {
            var dto = new OrderDetailsDto
            {
                Id = order.Id,
                Cost = order.Price.Value,
                CreateOrderDate = order.CreateOrderDate,
                CustomerId = order.CustomerId,
                OrderApprovedDate = order.OrderApprovedDate,
                OrderNumber = order.OrderNumber,
                Paid = order.Paid,
                UserId = order.UserId,
                Code = order.Currency.CurrencyCode,
                Rate = order.Currency.Rate,
                OrderItems = order.OrderItems.Select(oi => oi.AsDto()),
                Payments = order.Payments?.Select(p => p.AsDto())
            };
            return dto;
        }

        public static OrderItemDto AsDto(this OrderItem orderItem)
        {
            var dto = new OrderItemDto
            {
                Id = orderItem.Id,
                ItemCartId = orderItem.ItemCartId,
                UserId = orderItem.UserId,
                ItemCart = orderItem.ItemCart.AsDto(),
                Code = orderItem.Currency.CurrencyCode,
                Rate = orderItem.Currency.Rate,
                Cost = orderItem.Price.Value
            };
            return dto;
        }

        public static ItemCartDto AsDto(this ItemCart itemCart)
        {
            var dto = new ItemCartDto
            {
                Id = itemCart.Id,
                ItemName = itemCart.ItemName,
                BrandName = itemCart.BrandName,
                TypeName = itemCart.TypeName,
                Cost = itemCart.Price.Value,
                Description = itemCart.Description,
                Tags = itemCart.Tags,
                ImagesUrl = itemCart.ImagesUrl,
                Created = itemCart.Created,
                CurrencyCode = itemCart.CurrencyCode
            };
            return dto;
        }

        public static PaymentDto AsDto(this Payment payment)
        {
            var dto = new PaymentDto
            {
                Id = payment.Id,
                PaymentDate = payment.PaymentDate,
                PaymentNumber = payment.PaymentNumber,
                UserId = payment.UserId
            };
            return dto;
        }
    }
}
