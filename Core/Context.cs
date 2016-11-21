using System.Data.Entity;
using Core.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Core
{
    /// <summary>
    /// Контекст данных для работы с БД(запросить, изменить, удалить или вставить значения в базу данных)
    /// </summary>
    public class Context : IdentityDbContext<User>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Context()
            : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        static Context()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Migrations.Configuration>());
        }

        /// <summary>
        /// Создаёт контекст
        /// </summary>
        /// <returns>Контекст</returns>
        public static Context Create()
        {
            return new Context();
        }

        /// <summary>
        /// Интерфейс доступа к коллекции товаров
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Интерфейс доступа к коллекции заказов
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Интерфейс доступа к коллекции заказа товара
        /// </summary>
        public DbSet<OrderProduct> OrderProducts { get; set; }

        /// <summary>
        /// Интерфейс доступа к коллекции статистики
        /// </summary>
        public DbSet<Statistic> Statistics { get; set; }
    }
}
