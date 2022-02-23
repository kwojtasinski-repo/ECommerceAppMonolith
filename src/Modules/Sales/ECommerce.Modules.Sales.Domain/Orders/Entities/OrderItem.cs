using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Exceptions;

namespace ECommerce.Modules.Sales.Domain.Orders.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid ItemCartId { get; private set; }
        public ItemCart ItemCart { get; private set; }
        public Order Order { get; private set; }

        private OrderItem() { }

        public OrderItem(Guid id, Guid itemId, ItemCart itemCart)
        {
            ValidateItem(itemCart);
            Id = id;
            ItemCartId = itemId;
            ItemCart = itemCart;
        }

        public static OrderItem Create(ItemCart itemCart)
        {
            var order = new OrderItem(Guid.NewGuid(), itemCart.Id, itemCart);
            return order;
        }

        private static void ValidateItem(ItemCart itemCart)
        {
            if (itemCart is null)
            {
                throw new ItemCartCannotBeNullException();
            }
        }
    }
}
