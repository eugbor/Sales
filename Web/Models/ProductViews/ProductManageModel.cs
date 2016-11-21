using System.ComponentModel.DataAnnotations;
using System.Web;
using Core.Entities;

namespace Web.Models.ProductViews
{
    /// <summary>
    /// Модель менеджера для товара
    /// </summary>
    public class ProductManageModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        [Required]
        [Display(Name = "Стоимость")]
        public int Cost { get; set; }

        /// <summary>
        /// Размер
        /// </summary>
        [Display(Name = "Размер")]
        public Size Size { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        [Display(Name = "Файл")]
        public HttpPostedFileBase File { get; set; }
    }
}