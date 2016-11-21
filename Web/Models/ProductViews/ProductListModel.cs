using System.Collections.Generic;
using System.Linq;
using Core.Entities;

namespace Web.Models.ProductViews
{
    /// <summary>
    /// Модель списка товара
    /// </summary>
    public class ProductListModel
    {
        /// <summary>
        /// Список моделей
        /// </summary>
        public List<ProductListItemModel> Items { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="products">Товары</param>
        public ProductListModel(List<Product> products)
        {
            Items = products.Select(product => new ProductListItemModel(product)).ToList();
        }
    }
}