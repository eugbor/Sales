using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Core.Entities;

namespace Core.Managers
{
    /// <summary>
	/// Базовый менеджер
	/// </summary>
	/// <typeparam name="T">Тип сущности</typeparam>
    public abstract class BaseManager<T> : IDisposable
        where T : Entity
    {
        private DbSet<T> _set;

        /// <summary>
        /// Контекст доступа к БД
        /// </summary>
        protected Context Context { get; private set; }

        /// <summary>
        /// Проверка на ноль
        /// </summary>
        protected DbSet<T> Set => _set ?? (_set = Context.Set<T>());

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст БД</param>
        protected BaseManager(Context context)
        {
            Context = context;
        }

        /// <summary>
        /// Получает сущность из БД
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Сущность</returns>
        public virtual T Get(int id)
        {
            return Set.FirstOrDefault(el => el.Id == id);
        }

        /// <summary>
        /// Формирует список сущностей
        /// </summary>
        /// <returns>Список сущностей</returns>
        public virtual List<T> GetList()
        {
            return Set.ToList();
        }

        /// <summary>
        /// Добавляет объект в БД
        /// </summary>
        /// <param name="obj">Объект</param>
        public virtual void Save(T obj)
        {
            if (obj.Id <= 0)
                Set.Add(obj);

            Context.SaveChanges();
        }

        /// <summary>
        /// Удаляет сущность из БД по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public virtual void Remove(int id)
        {
            var obj = Get(id);
            if (obj != null)
                Set.Remove(obj);

            Context.SaveChanges();
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public virtual void Dispose()
        {
            Context = null;
        }
    }
}
