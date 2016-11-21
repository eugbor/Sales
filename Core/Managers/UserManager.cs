using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Core.Managers
{
    /// <summary>
	/// Менеджер для работы с пользователями
	/// </summary>
    public class UserManager : UserManager<User, String>
    {
        /// <summary>
        /// Контекст
        /// </summary>
        private Context Context { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст</param>
        internal UserManager(Context context)
            :this(new UserStore<User>(context))
        {
            Context = context;
        }

        /// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="store">Хранилище</param>
        internal UserManager(IUserStore<User> store)
            : base(store)
        {
        }

        /// <summary>
		/// Создание экземпляра менеджера
		/// </summary>
		/// <param name="options">Опции</param>
		/// <param name="owinContext">Контекст</param>
		/// <returns>Менеджер</returns>
        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
        {
            var manager = new UserManager(context.Get<Context>());

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = true
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
                manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("Identity"));

            return manager;
        }

        /// <summary>
        /// Создает пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Сохраненный пользователь</returns>
        public override Task<IdentityResult> CreateAsync(User user)
        {
            user.CreatedDate = DateTime.Now;
            return base.CreateAsync(user);
        }

        /// <summary>
        /// Формирует список пользователей по роле
        /// </summary>
        /// <param name="roleName">Наименование роли</param>
        /// <returns>Список пользователей по роле</returns>
        public List<User> GetListByRole(string roleName)
        {
            var role = Context.Roles.FirstOrDefault(el => el.Name == roleName);
            if (role == null)
                return new List<User>();

            return Users.Where(el => el.Roles.Any(r => r.RoleId == role.Id)).ToList();
        }

        /// <summary>
        /// Изменяет пороль
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="newPassword">Новый пароль</param>
        /// <returns>Новый сохраненный пароль</returns>
        public async Task<IdentityResult> ChangePasswordAsync(string id, string newPassword)
        {
            User user = await FindByIdAsync(id);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var store = this.Store as IUserPasswordStore<User, String>;
            if (store == null)
            {
                var errors = new string[] { "Current UserStore doesn't implement IUserPasswordStore" };
                return IdentityResult.Failed(errors);
            }

            var newPasswordHash = this.PasswordHasher.HashPassword(newPassword);
            await store.SetPasswordHashAsync(user, newPasswordHash);
            await store.UpdateAsync(user);
            return IdentityResult.Success;
        }
        
    }
}
