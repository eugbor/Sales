using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core;
using Core.Entities;
using Core.Managers;
using Microsoft.AspNet.Identity.Owin;
using Web.Models.OrderViews;
using Web.Models.ProductViews;

namespace Web.Controllers
{
    /// <summary>
    /// Контроллер сотрудника фирмы
    /// </summary>
    [Authorize(Roles = Roles.Employee)]
    public class EmployeeController : Controller
    {
        /// <summary>
        /// Поле менеджера для товара
        /// </summary>
        private ProductManager _productManager;

        /// <summary>
        /// Поле менеджера для заказа
        /// </summary>
        private OrderManager _orderManager;

        /// <summary>
        /// Свойство менеджера для товара
        /// </summary>
        public ProductManager ProductManager
        {
            get { return _productManager ?? HttpContext.GetOwinContext().Get<ProductManager>(); }
            private set { _productManager = value; }
        }

        /// <summary>
        /// Свойство менеджера для заказа
        /// </summary>
        public OrderManager OrderManager
        {
            get { return _orderManager ?? HttpContext.GetOwinContext().Get<OrderManager>(); }
            private set { _orderManager = value; }
        }

        /// <summary>
        /// GET: Представляет личный кабинет сотрудника фирмы
        /// </summary>
        /// <returns>Личный кабинет сотрудника фирмы</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: Формирует список товаров
        /// </summary>
        /// <param name="minCost">Минимальная стоимость</param>
        /// <param name="maxCost">Максимальная стоимость</param>
        /// <param name="size">Размер товара</param>
        /// <returns>Список товаров</returns>
        public ActionResult ProductList(int? minCost, int? maxCost, Size? size)
        {
            var products = ProductManager.GetList(minCost, maxCost, size);
            ProductListModel model = new ProductListModel(products);
            return View(model);
        }

        /// <summary>
        /// GET: Формирует список заказов
        /// </summary>
        /// <param name="statuses">Статус заказа</param>
        /// <returns>Список заказов</returns>
        public ActionResult OrderList(Status[] statuses)
        {
            var orders = OrderManager.GetList(statuses);
            OrderListModel model = new OrderListModel(orders);
            return View(model);
        }
        /// <summary>
        /// GET: Редактирует заказ
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Заказ</returns>
        public async Task<ActionResult> OrderEdit(int id)
        {
            var order = OrderManager.Get(id);
            if (order == null)
                return RedirectToAction("OrderList", "Employee");

            var model = new OrderManagerModel
            {
                Id = order.Id,
                UserEmail = order.User.Email,
                Date = order.Date,
                Status = order.Status,
            };

            OrderManager.Save(order);

            return View(model);
        }

        /// <summary>
        /// POST: Редактирует заказ
        /// </summary>
        /// <param name="model">Модель</param>
        /// <returns>Заказ</returns>
        [HttpPost]
        public async Task<ActionResult> OrderEdit(OrderManagerModel model)
        {
            if (model.Id == 0)
                return View(model);
            
            var order = OrderManager.Get(model.Id);
            if (order == null)
                return View(model);

            order.Date = model.Date;
            order.Status = model.Status;
            
            OrderManager.Save(order);


            return RedirectToAction("OrderList", "Employee");
        }

        /// <summary>
        /// POST: Запрашивает подтверждение на отмену заказа
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Подтверждение</returns>
        [HttpGet]
        public async Task<ActionResult> OrderCancel(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = OrderManager.Get(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View("OrderCancel", (object)id);
        }

        /// <summary>
        /// POST: После подтверждения отменяет заказ
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Список заказов</returns>
        [HttpPost]
        public async Task<ActionResult> CancelConfirmed(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = OrderManager.Get(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            OrderManager.Remove(id);

            return RedirectToAction("OrderList", "Employee");
        }
    }
}