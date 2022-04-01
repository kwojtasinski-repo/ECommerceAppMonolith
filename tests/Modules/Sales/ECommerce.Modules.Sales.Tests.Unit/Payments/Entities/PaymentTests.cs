using ECommerce.Modules.Sales.Domain.Orders.Common.ValueObjects;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Payments.Entities;
using ECommerce.Modules.Sales.Domain.Payments.Exceptions;
using Shouldly;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Unit.Payments.Entities
{
    public class PaymentTests
    {
        [Fact]
        public void should_create_payment()
        {
            var userId = Guid.NewGuid();
            var currency = Currency.Default();
            var order = Order.Create(Guid.NewGuid(), "ORD/2022/03/03/1", 1500M, currency.CurrencyCode, currency.Rate, Guid.NewGuid(), userId, DateTime.Now);

            var payment = Payment.Create(Guid.NewGuid(), "PAY/2022/03/03/1", order, userId, DateTime.Now);

            payment.ShouldNotBeNull();
            payment.UserId.ShouldBe(userId);
            payment.Order.OrderNumber.ShouldBe(order.OrderNumber);
        }

        [Fact]
        public void given_invalid_payment_number_should_throw_an_exception()
        {
            var userId = Guid.NewGuid();
            var currency = Currency.Default();
            var order = Order.Create(Guid.NewGuid(), "ORD/2022/03/03/1", 1500M, currency.CurrencyCode, currency.Rate, Guid.NewGuid(), userId, DateTime.Now);
            var expectedException = new PaymentNumberCannotBeEmptyException();

            var exception = Record.Exception(() => Payment.Create(Guid.NewGuid(), "", order, userId, DateTime.Now));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<PaymentNumberCannotBeEmptyException>();
            exception.Message.ShouldBe(exception.Message);
        }
    }
}