using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Web.Models.OrderViews
{
    /// <summary>
    /// Модель менеджера для заказа
    /// </summary>
    public class OrderManagerModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Имя клиента
        /// </summary>
        [Required]
        [Display(Name = "Имя клиента")]
        public string UserEmail { get; set; }

        /// <summary>
        /// Дата заказа
        /// </summary>
        [Display(Name = "Дата заказа")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [Required]
        [Display(Name = "Статус")]
        public Status Status { get; set; }
    }
}