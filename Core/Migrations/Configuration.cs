using Core.Entities;
using Core.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    /// <summary>
    /// Конфигурация
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<Core.Context>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }

        /// <summary>
        /// Формирует контекст
        /// </summary>
        /// <param name="context">Контекст</param>
        protected override void Seed(Core.Context context)
        {
            if (context.Roles.FirstOrDefault(role => role.Name == Core.Roles.Admin) == null)
                context.Roles.Add(new IdentityRole(Core.Roles.Admin));
            if (context.Roles.FirstOrDefault(role => role.Name == Core.Roles.Employee) == null)
                context.Roles.Add(new IdentityRole(Core.Roles.Employee));
            if (context.Roles.FirstOrDefault(role => role.Name == Core.Roles.User) == null)
                context.Roles.Add(new IdentityRole(Core.Roles.User));

            var userManager = new UserManager(context);
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //roleManager.Create(new IdentityRole { Name = "Admin"});
            //roleManager.Create(new IdentityRole { Name = "Employee" });
            //roleManager.Create(new IdentityRole { Name = "User" });

            // создаем пользователей
            if (userManager.FindByEmail("somemail@mail.ru") == null)
            {
                var admin = new User {Email = "somemail@mail.ru", UserName = "somemail@mail.ru"};
                string password = "123456";
                var result = userManager.Create(admin, password);

                // если создание пользователя прошло успешно
                if (result.Succeeded)
                {
                    // добавляем для пользователя роль
                    userManager.AddToRole(admin.Id, Core.Roles.Admin);
                }
            }

            base.Seed(context);
        }
    }
}
