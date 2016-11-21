using System.Collections.Generic;

namespace Web.Models.AdminViews
{
    /// <summary>
    /// Модель статистики
    /// </summary>
    public class StatisticModel
    {
        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Просмотры
        /// </summary>
        public Dictionary<string, int> Views { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="count">Количество</param>
        /// <param name="views">Просмотры</param>
        public StatisticModel(int count, Dictionary<string, int> views)
        {
            Count = count;
            Views = views;
        }
    }
}