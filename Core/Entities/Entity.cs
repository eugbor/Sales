using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    /// <summary>
    /// Интерфейс базовой сущности
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntity<T>
    {
        T Id { get; set; }
    }

    /// <summary>
    /// Базовая сущность
    /// </summary>
    public abstract class Entity : IEntity<int>
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}
