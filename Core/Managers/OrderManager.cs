using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Core.Managers
{
    /// <summary>
    /// Менеджер для работы с заказами
    /// </summary>
    public class OrderManager : BaseManager<Order>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст</param>
        internal OrderManager(Context context)
            : base(context)
        {
        }

        /// <summary>
        /// Создание экземпляра менеджера
        /// </summary>
        /// <param name="options">Опции</param>
        /// <param name="owinContext">Контекст</param>
        /// <returns>Менеджер</returns>
        public static OrderManager Create(IdentityFactoryOptions<OrderManager> options, IOwinContext owinContext)
        {
            var context = owinContext.Get<Context>();

            return new OrderManager(context);
        }

        /// <summary>
        /// Формирует список запросов
        /// </summary>
        /// <param name="statuses">Состояния</param>
        /// <returns>Список запросов</returns>
        public List<Order> GetList(params Status[] statuses)
        {
            var query = Set.AsQueryable();

            if (statuses != null)
                query = query.Where(el => statuses.Contains(el.Status));

            return query.ToList();

        }

        /// <summary>
        /// Формирует список имен пользователей
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>Список имен пользователей</returns>
        public List<Order> GetList(string userName)
        {
            var query = Set.AsQueryable();

            if (userName != null)
                query = query.Where(el => el.User.UserName == userName);

            return query.ToList();
        }

        /// <summary>
        /// Удаляет заказ
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public override void Remove(int id)
        {
            var order = Get(id);
            if (order == null)
                return;

            Context.OrderProducts.RemoveRange(order.Products);

            Context.Orders.Remove(order);

            Context.SaveChanges();
        }
    }
}
