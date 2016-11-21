using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Entities;
using Core.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Web.Models;
using Web.Models.AdminViews;
using Roles = Core.Roles;

namespace Web.Controllers
{
    /// <summary>
    /// Контроллер администратора
    /// </summary>
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        /// <summary>
        /// Поле менеджера для входа
        /// </summary>
        private ApplicationSignInManager _signInManager;

        /// <summary>
        /// Поле менеджера для пользователя
        /// </summary>
        private UserManager _userManager;

        /// <summary>
        /// Поле менеджера для статистики
        /// </summary>
        private StatisticManager _statisticManager;

        /// <summary>
        /// Свойство менеджера для входа
        /// </summary>
        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        /// <summary>
        /// Свойство менеджера для пользователя
        /// </summary>
        public UserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
            private set { _userManager = value; }
        }

        /// <summary>
        /// Свойство менеджера для статистики
        /// </summary>
        public StatisticManager StatisticManager
        {
            get { return _statisticManager ?? HttpContext.GetOwinContext().GetUserManager<StatisticManager>(); }
            private set { _statisticManager = value; }
        }

        /// <summary>
        /// Добавляет ошибки
        /// </summary>
        /// <param name="result">Результат</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// GET: Представляет личный кабинет администратора
        /// </summary>
        /// <returns>Личный кабинет администратора</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: Формирует список пользователей
        /// </summary>
        /// <param name="role">Роль</param>
        /// <returns>Список пользователей</returns>
        public ActionResult UserList(string role)
        {
            if (string.IsNullOrEmpty(role))
                role = Roles.Employee;

            var users = UserManager.GetListByRole(role);
            UserListModel model = new UserListModel(users);
            return View(model);
        }

        /// <summary>
        /// GET: Создает пользователя
        /// </summary>
        /// <returns>Создать пользователя</returns>
        public ActionResult UserCreate()
        {
            return View();
        }
        
        /// <summary>
        /// POST: Создает пользователя
        /// </summary>
        /// <param name="model">Модель</param>
        /// <returns>Список пользователей</returns>
        [HttpPost]
        public async Task<ActionResult> UserCreate(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User {UserName = model.Email, Email = model.Email};
            var result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            if (model.Role == 0)
                await UserManager.AddToRolesAsync(user.Id, Roles.Employee);
            else
                await UserManager.AddToRolesAsync(user.Id, Roles.User);

            return RedirectToAction("UserList", "Admin");
        }

        /// <summary>
        /// GET: Редактирует пользователя
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Редактировать пользователя</returns>
        public async Task<ActionResult> UserEdit(string id)
        {
            var model = new RegisterViewModel();

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return RedirectToAction("UserList", "Admin");

            model.Id = user.Id;
            model.Email = user.Email;
            model.Role = await UserManager.IsInRoleAsync(user.Id, Roles.Employee) ? 0 : 1;
            
            return View(model);
        }

        /// <summary>
        /// POST: Редактирует пользователя
        /// </summary>
        /// <param name="model">Модель</param>
        /// <returns>Список пользователей</returns>
        [HttpPost]
        public async Task<ActionResult> UserEdit(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == null)
                return View(model);

            var user = await UserManager.FindByIdAsync(model.Id);
            if (user == null)
                return View(model);

            var result = await UserManager.ChangePasswordAsync(model.Id, model.Password);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            var userRoles = (await UserManager.GetRolesAsync(user.Id)).ToArray();
            result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            if (model.Role == 0)
                await UserManager.AddToRolesAsync(user.Id, Roles.Employee);
            else
                await UserManager.AddToRolesAsync(user.Id, Roles.User);

            return RedirectToAction("UserList", "Admin");
        }

        /// <summary>
        /// POST: Запрашивает подтверждение на удаление пользователя
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Подтверждение</returns>
        [HttpGet]
        public async Task<ActionResult> UserDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View("UserDelete", (object)id);
        }

        /// <summary>
        /// Post: После подтверждения удаляет пользователя
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Список пользователей</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            await UserManager.DeleteAsync(user);

            return RedirectToAction("UserList", "Admin");
        }

        /// <summary>
        /// GET: Формирует статистику
        /// </summary>
        /// <returns>Статистика</returns>
        public ActionResult Statistics()
        {
            var count = StatisticManager.GetVisitsByLastDay();
            var visits = StatisticManager.GetGroupVisitsByTimeForLastDay();
            var model = new StatisticModel(count, visits);

            return View(model);
        }
    }
}