using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;

namespace Web.Models.AdminViews
{
    /// <summary>
    /// Модель списка пользователя
    /// </summary>
    public class UserListModel
    {
        /// <summary>
        /// Список моделей
        /// </summary>
        public List<UserListItemModel> Items { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="users">Список пользователей</param>
        public UserListModel(List<User> users)
        {
            Items = users.Select(user => new UserListItemModel(user)).ToList();
        }
    }

    /// <summary>
    /// Предметная модель списка пользователей 
    /// </summary>
    public class UserListItemModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Полное имя
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="user">Пользователь</param>
        public UserListItemModel(User user)
        {
            Id = user.Id;
            FullName = $"{user.FirstName} {user.LastName}";
            Email = user.Email;
            Date = user.CreatedDate;
        }
    }
}