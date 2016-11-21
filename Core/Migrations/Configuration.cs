using Core.Entities;
using Core.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    /// <summary>
    /// ������������
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<Core.Context>
    {
        /// <summary>
        /// �����������
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }

        /// <summary>
        /// ��������� ��������
        /// </summary>
        /// <param name="context">��������</param>
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

            // ������� �������������
            if (userManager.FindByEmail("somemail@mail.ru") == null)
            {
                var admin = new User {Email = "somemail@mail.ru", UserName = "somemail@mail.ru"};
                string password = "123456";
                var result = userManager.Create(admin, password);

                // ���� �������� ������������ ������ �������
                if (result.Succeeded)
                {
                    // ��������� ��� ������������ ����
                    userManager.AddToRole(admin.Id, Core.Roles.Admin);
                }
            }

            base.Seed(context);
        }
    }
}
