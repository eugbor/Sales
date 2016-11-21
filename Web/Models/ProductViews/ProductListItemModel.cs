using Core.Entities;

namespace Web.Models.ProductViews
{
    /// <summary>
    /// Модель списка товара
    /// </summary>
    public class ProductListItemModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Размер
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="product">Товар</param>
        public ProductListItemModel(Product product)
        {
            Id = product.Id;
            Title = product.Title;
            Description = product.Description;
            Cost = product.Cost;
            switch (product.Size)
            {
                case Core.Entities.Size.Small:
                    Size = "Маленький";
                    break;
                case Core.Entities.Size.Middle:
                    Size = "Средний";
                    break;
                case Core.Entities.Size.Big:
                    Size = "Большой";
                    break;
            }
        }
    }
}