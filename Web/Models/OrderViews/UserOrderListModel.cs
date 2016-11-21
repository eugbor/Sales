using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;

namespace Web.Models.OrderViews
{
    /// <summary>
    /// Модель списка заказа для пользователя
    /// </summary>
    public class UserOrderListModel
    {
        /// <summary>
        /// Список моделей
        /// </summary>
        public List<UserOrderListItemModel> Items { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="orders">Заказы</param>
        public UserOrderListModel(List<Order> orders)
        {
            Items = orders.Select(order => new UserOrderListItemModel(order)).ToList();
        }
    }

    /// <summary>
    /// Предметная модель списка заказа для пользователя
    /// </summary>
    public class UserOrderListItemModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Email пользователя
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="order">Заказ</param>
        public UserOrderListItemModel(Order order)
        {
            Id = order.Id;
            UserEmail = order.User.Email;
            Date = order.Date;

            switch (order.Status)
            {
                case Core.Entities.Status.New:
                    Status = "Новый заказ";
                    break;
                case Core.Entities.Status.Processing:
                    Status = "В процессе";
                    break;
                case Core.Entities.Status.Done:
                    Status = "Выполнен";
                    break;
                case Core.Entities.Status.Error:
                    Status = "Ошибка";
                    break;
            }
        }
    }
}