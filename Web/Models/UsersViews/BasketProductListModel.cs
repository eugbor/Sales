using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Web.Models.ProductViews;

namespace Web.Models.UsersVIews
{
    /// <summary>
    /// Модель списка корзины с товаром 
    /// </summary>
    public class BasketProductListModel
    {
        /// <summary>
        /// Список моделей
        /// </summary>
        public List<BasketProductListItemModel> Items { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public int Sum { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="products">Товары</param>
        /// <param name="basket">Корзина</param>
        public BasketProductListModel(List<Product> products, BasketModel basket)
        {
            Items = products.Select(product => new BasketProductListItemModel(product, basket.Products[product.Id])).ToList();
            Sum = Items.Sum(el => el.Cost*el.Count);
        }
    }

    /// <summary>
    /// Предметная модель списка корзины с товаром 
    /// </summary>
    public class BasketProductListItemModel : ProductListItemModel
    {
        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="product">Товар</param>
        /// <param name="count">Количество</param>
        public BasketProductListItemModel(Product product, int count)
            : base(product)
        {
            Count = count;
        }

    }
}