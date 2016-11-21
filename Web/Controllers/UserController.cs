using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Entities;
using Core.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Web.Models.OrderViews;
using Web.Models.UsersVIews;

namespace Web.Controllers
{
    /// <summary>
    /// Контроллер пользователя (клиента)
    /// </summary>
    [Authorize(Roles = Core.Roles.User)]
    public class UserController : Controller
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
        /// Поле менеджера для пользователя
        /// </summary>
        private UserManager _userManager;

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
        /// Свойство менеджера для пользователя
        /// </summary>
        public UserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
            private set { _userManager = value; }
        }

        /// <summary>
        /// GET: Представляет личный кабинет клиента
        /// </summary>
        /// <returns>Личный кабинет клиента</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET:Добавляет продукт в корзину
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Товар</returns>
        public ActionResult BasketAddProduct(int id)
        {
            bool existCookie;
            var basket = GetBasket(out existCookie);

            basket.AddProduct(id);

            if (!existCookie)
                Response.Cookies.Add(new HttpCookie("Basket", JsonConvert.SerializeObject(basket)));
            else
                Response.Cookies["Basket"].Value = JsonConvert.SerializeObject(basket);

            return Json("Товар добавлен в корзину", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GET: Удаляет продукт из корзины
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Корзина</returns>
        public ActionResult BasketRemoveProduct(int id)
        {
            bool existCookie;
            var basket = GetBasket(out existCookie);

            basket.RemoveProduct(id);

            if (existCookie)
                Response.Cookies["Basket"].Value = JsonConvert.SerializeObject(basket);


            return Json("Товар удалён из корзины", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GET: Формирует список продуктов в корзине
        /// </summary>
        /// <returns>Список продуктов в корзине</returns>
        public ActionResult Basket()
        {
            bool existCookie;
            var basket = GetBasket(out existCookie);

            var products = ProductManager.GetList(basket.Products.Keys.ToList());

            var model = new BasketProductListModel(products, basket);

            return View(model);
        }

        /// <summary>
        /// GET: Получает корзину
        /// </summary>
        /// <param name="existCookie">Фрагмент данных</param>
        /// <returns>Корзина</returns>
        private BasketModel GetBasket(out bool existCookie)
        {
            var basketCookie = Request.Cookies["Basket"];
            BasketModel basket;
            existCookie = basketCookie != null && basketCookie.Value != null;
            if (!existCookie)
                basket = new BasketModel();
            else
                basket = JsonConvert.DeserializeObject<BasketModel>(basketCookie.Value);

            return basket;
        }

        /// <summary>
        /// GET: Формирует список заказов клиента
        /// </summary>
        /// <returns>Список заказов клиента</returns>
        public ActionResult MyOrdersList()
        {
            var orders = OrderManager.GetList(User.Identity.Name);

            UserOrderListModel model = new UserOrderListModel(orders);

            return View(model);
        }

        /// <summary>
        /// GET: Совершает обработку по созданию заказа
        /// </summary>
        /// <returns>Заказ</returns>
        public ActionResult Thanks()
        {
            bool existBastek;
            var basket = GetBasket(out existBastek);

            var user = UserManager.FindByEmail(User.Identity.Name);

            Order order = new Order
            {
                User = user,
                Date = DateTime.Now,
                Status = Status.New
            };

            foreach (var pair in basket.Products)
            {
                var product = ProductManager.Get(pair.Key);
                order.Add(product, pair.Value);
            }

            OrderManager.Save(order);

            basket.Products.Clear();
            if (!existBastek)
                Response.Cookies.Add(new HttpCookie("Basket", JsonConvert.SerializeObject(basket)));
            else
                Response.Cookies["Basket"].Value = JsonConvert.SerializeObject(basket);

            return View();
        }
    }
}