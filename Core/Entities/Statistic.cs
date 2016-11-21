using System;

namespace Core.Entities
{
    /// <summary>
    /// Статистика
    /// </summary>
    public class Statistic : Entity
    {
        /// <summary>
        /// Дата посещения
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Определитель местонахождения
        /// </summary>
        public string Url { get; set; }
    }
}
