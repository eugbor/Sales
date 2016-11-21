using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Core.Managers
{
    /// <summary>
	/// Менеджер для работы со статистикой
	/// </summary>
    public class StatisticManager : BaseManager<Statistic>
    {
        /// <summary>
        /// Поле моего контекста
        /// </summary>
        private bool _isMineContext;
        /// <summary>
		/// Конструктор
		/// </summary>
        public StatisticManager()
            : base(new Context())
        {
            _isMineContext = true;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        internal StatisticManager(Context context)
            : base(context)
        {
        }

        /// <summary>
        /// Создание экземпляра менеджера
        /// </summary>
        /// <param name="options">Опции</param>
        /// <param name="owinContext">Контекст</param>
        /// <returns>Менеджер</returns>
        public static StatisticManager Create(IdentityFactoryOptions<StatisticManager> options, IOwinContext owinContext)
        {
            var context = owinContext.Get<Context>();

            return new StatisticManager(context);
        }

        /// <summary>
        /// Формирует количество посителей за день
        /// </summary>
        /// <returns></returns>
        public int GetVisitsByLastDay()
        {
            var minDate = DateTime.Now.AddDays(-1);
            var maxDate = DateTime.Now;

            return Context.Statistics.Count(el => el.Date >= minDate && el.Date <= maxDate);
        }

        /// <summary>
        /// Формирует коллекцию посений за день
        /// </summary>
        /// <returns>Контест посений за день</returns>
        public Dictionary<string, int> GetGroupVisitsByTimeForLastDay()
        {
            var minDate = DateTime.Now.AddDays(-1);
            var maxDate = DateTime.Now;

            return
                Context.Statistics.Where(el => el.Date >= minDate && el.Date <= maxDate).OrderBy(el=>el.Date).AsEnumerable()
                    .Select(el => el.Date.ToString("HH:00"))
                    .GroupBy(el => el)
                    .ToDictionary(el => el.Key, el => el.Count());
        }

        /// <summary>
        /// Удаляет контекст
        /// </summary>
        public override void Dispose()
        {
            if (_isMineContext)
                Context.Dispose();
            base.Dispose();
        }
    }
}
