using System.Collections.Generic;
using System.Linq;
using Core.Entities;

namespace Web.Models.OrderViews
{
    /// <summary>
    /// Модель списка заказа
    /// </summary>
    public class OrderListModel
    {
        /// <summary>
        /// Список моделей
        /// </summary>
        public List<OrderListItemModel> Items { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="orders">Заказ</param>
        public OrderListModel(List<Order> orders)
        {
            Items = orders.Select(order => new OrderListItemModel(order)).ToList();
        }
    }

    /// <summary>
    /// Предметная модель списка пользователей 
    /// </summary>
    public class OrderListItemModel: UserOrderListItemModel
    {
        /// <summary>
        /// Товары
        /// </summary>
        public virtual List<OrderProduct> Products { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="order">Заказ</param>
        public OrderListItemModel(Order order)
            : base(order)
        {
            Products = order.Products;
        }
    }
}