using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Core.Managers
{
    /// <summary>
	/// Менеджер для работы с товарами
	/// </summary>
    public class ProductManager : BaseManager<Product>
    {
        /// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="context">Контекст</param>
        internal ProductManager(Context context)
            : base(context)
        {
        }

        /// <summary>
		/// Создание экземпляра менеджера
		/// </summary>
		/// <param name="options">Опции</param>
		/// <param name="owinContext">Контекст</param>
		/// <returns>Менеджер</returns>
        public static ProductManager Create(IdentityFactoryOptions<ProductManager> options, IOwinContext owinContext)
        {
            var context = owinContext.Get<Context>();

            return new ProductManager(context);
        }

        /// <summary>
        /// Формирует список товаров 
        /// </summary>
        /// <param name="minCost">Минимальная стоимость</param>
        /// <param name="maxCost">Максимальная стоимость</param>
        /// <param name="size">Размер товара</param>
        /// <returns>Список запросов</returns>
        public List<Product> GetList(int? minCost, int? maxCost, Size? size)
        {
            var query = Set.AsQueryable();

            if (minCost != null)
                query = query.Where(el => el.Cost >= minCost.Value);

            if (maxCost != null)
                query = query.Where(el => el.Cost <= maxCost.Value);

            if (size != null)
                query = query.Where(el => el.Size == size.Value);

            return query.ToList();
        }

        /// <summary>
        /// Формирует список товаров
        /// </summary>
        /// <param name="listId">Список идентификаторов</param>
        /// <returns>Список продуктов</returns>
        public List<Product> GetList(List<int> listId)
        {
            var query = Set.AsQueryable();

            if (listId != null)
                query = query.Where(el => listId.Contains(el.Id));

            return query.ToList();
        } 
    }
}
