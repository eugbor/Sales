using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Entities
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class Order : Entity
    {
        /// <summary>
        /// Пользоваиель, который сделал заказ
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Дата создания заказа
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Статус заказа, в каком состаянии находится заказ
        /// </summary>
        [Index]
        public Status Status { get; set; }

        /// <summary>
        /// Список заказанных продуктов
        /// </summary>
        public virtual List<OrderProduct> Products { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Order()
        {
            Products = new List<OrderProduct>();
        }

        /// <summary>
        /// Добавляет продукт в заказ
        /// </summary>
        /// <param name="product">Товар</param>
        /// <param name="count">Количество</param>
        public void Add(Product product, int count)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var orderProduct = Products.FirstOrDefault(el => el.Product.Id == product.Id);

            if (orderProduct != null)
            {
                orderProduct.Count += count;
            }

            else
            {
                orderProduct = new OrderProduct
                {
                    Count = count,
                    Price = product.Cost,
                    Product = product
                };
                Products.Add(orderProduct);
            }
        }

        /// <summary>
        /// Удаляет продукт из заказа
        /// </summary>
        /// <param name="product">Товар</param>
        /// <param name="count">Количество</param>
        public void Remove(Product product, int count)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var orderProduct = Products.FirstOrDefault(el => el.Product.Id == product.Id);

            if (orderProduct != null)
            {
                orderProduct.Count -= count;
                if (orderProduct.Count <= 0)
                    Products.Remove(orderProduct);
            }
        }

    }
}
