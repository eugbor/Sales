using System.Collections.Generic;
using System.Linq;
using Core.Entities;

namespace Web.Models.ProductViews
{
    /// <summary>
    /// Модель списка товара на главной странице
    /// </summary>
    public class ProductHomeListModel
    {
        /// <summary>
        /// Список моделей
        /// </summary>
        public List<ProductHomeListItemModel> Items { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="products">Товары</param>
        public ProductHomeListModel(List<Product> products)
        {
            Items = products.Select(product => new ProductHomeListItemModel(product)).ToList();
        }
    }

    /// <summary>
    /// Предметная модель списка товара на главной странице
    /// </summary>
    public class ProductHomeListItemModel:ProductListItemModel
    {
        /// <summary>
        /// Название картинки 
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="product">Товар</param>
        public ProductHomeListItemModel(Product product)
            :base(product)
        {
            ImageName = product.ImageName;
        }
    }


}