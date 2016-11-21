using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    /// <summary>
    /// Товар
    /// </summary>
    public class Product : Entity
    {
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
        [Index]
        public Size Size { get; set; }

        /// <summary>
        /// Название картинки
        /// </summary>
        public string ImageName { get; set; }
    }
}
