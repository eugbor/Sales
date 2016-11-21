using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Web.Models.UsersVIews;

namespace Web.Models.ProductViews
{
    /// <summary>
    /// Модель списка заказанного товара
    /// </summary>
    public class OrderProductListModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public int Sum { get; set; }

        /// <summary>
        /// Список моделей
        /// </summary>
        public List<OrderProductListItemModel> Items { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="order">Заказ</param>
        public OrderProductListModel(Order order)
        {
            Id = order.Id;

            Items = order.Products.Select(el => new OrderProductListItemModel(el.Product, el.Count)).ToList();

            Sum = Items.Sum(el => el.Cost * el.Count);
        }
    }

    /// <summary>
    /// Предметная модель списка заказанного товара
    /// </summary>
    public class OrderProductListItemModel : BasketProductListItemModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="product">Товар</param>
        /// <param name="count">Количество</param>
        public OrderProductListItemModel(Product product, int count) 
            : base(product, count)
        {
        }
    }
}