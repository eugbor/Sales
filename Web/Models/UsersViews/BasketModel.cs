using System.Collections.Generic;

namespace Web.Models.UsersVIews
{
    /// <summary>
    /// Модель корзины
    /// </summary>
    public class BasketModel
    {
        /// <summary>
        /// Товары
        /// </summary>
        public Dictionary<int, int> Products;

        /// <summary>
        /// Конструктор
        /// </summary>
        public BasketModel()
        {
            Products = new Dictionary<int, int>();
        }

        /// <summary>
        /// Добавляет один товар
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public void AddProduct(int id)
        {
            if (Products.ContainsKey(id))
                Products[id]++;
            else
                Products.Add(id, 1);
        }

        /// <summary>
        /// Удаляет один товар
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public void RemoveProduct(int id)
        {
            if (!Products.ContainsKey(id))
                return;

            if (Products[id] > 1)
                Products[id]--;
            else
                Products.Remove(id);
        }
    }
}