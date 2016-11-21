using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Entities;
using Core.Managers;
using Microsoft.AspNet.Identity.Owin;
using Web.Models.ProductViews;

namespace Web.Controllers
{
    /// <summary>
    /// Контроллер продукта
    /// </summary>
    public class ProductController : Controller
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
        /// GET: Добавляет товар
        /// </summary>
        /// <returns>Модель</returns>
        public ActionResult ProductAdd()
        {
            return View(new ProductManageModel());
        }

        /// <summary>
        /// POST: Добавляет товар
        /// </summary>
        /// <param name="model">Модель</param>
        /// <returns>Список товаров</returns>
        [HttpPost]
        public async Task<ActionResult> ProductAdd(ProductManageModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(model.File.FileName))
                return View(model);

            var imageName = Guid.NewGuid().ToString("N") + "." + model.File.FileName.Split('.').Last();

            var product = new Product
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Cost = model.Cost,
                Size = model.Size,
                ImageName = imageName,
            };

            ProductManager.Save(product);
            ProductFileSave(imageName, model.File);
            
            return RedirectToAction("ProductList", "Employee");
        }

        /// <summary>
        /// GET: Редактирует товар
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Товар</returns>
        public async Task<ActionResult> ProductEdit(int id)
        {
            var product = ProductManager.Get(id);
            if (product == null)
                return RedirectToAction("ProductList", "Employee");

            var model = new ProductManageModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Cost = product.Cost,
                Size = product.Size,
            };

            return View(model);
        }

        /// <summary>
        /// POST: Редактирует товар
        /// </summary>
        /// <param name="model">Модель</param>
        /// <returns>Товар</returns>
        [HttpPost]
        public async Task<ActionResult> ProductEdit(ProductManageModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
                return View(model);

            var product = ProductManager.Get(model.Id);
            if (product == null)
                return View(model);

            product.Title = model.Title;
            product.Description = model.Description;
            product.Cost = model.Cost;
            product.Size = model.Size;

            ProductManager.Save(product);

            if (!string.IsNullOrEmpty(model.File.FileName))
            {
                var imageName = Guid.NewGuid().ToString("N") + "." + model.File.FileName.Split('.').Last();
                ProductFileSave(imageName, model.File);
            }

            return RedirectToAction("ProductList", "Employee");
        }

        /// <summary>
        /// POST: Запрашивает подтверждение на удаление товара
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Подтверждение</returns>
        [HttpGet]
        public async Task<ActionResult> ProductDelete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = ProductManager.Get(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View("ProductDelete", (object)id);
        }

        /// <summary>
        /// POST: После подтверждения удаляет товар
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Список товаров</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = ProductManager.Get(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ProductManager.Remove(id);

            return RedirectToAction("ProductList", "Employee");
        }

        /// <summary>
        /// Сохраняет картинку товара
        /// </summary>
        /// <param name="imageName">Название картинки</param>
        /// <param name="file">Файл</param>
        private void ProductFileSave(string imageName, HttpPostedFileBase file)
        {
            string path = Path.Combine(Server.MapPath("~/Images"), imageName);
            file.SaveAs(path);
        }

        /// <summary>
        /// GET: Формирует список заказанных товаров клиентом
        /// </summary>
        /// <param name="order">Заказ</param>
        /// <returns>Список заказанных товаров клиента</returns>
        public ActionResult List(int order)
        {
            var orderEntity = OrderManager.Get(order);
            var model = new OrderProductListModel(orderEntity);

            return View("OrderProductList", model);
        }

        /// <summary>
        /// POST: Запрашивает подтверждение на удаление заказанного товара клиеном
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Подтверждение</returns>
        [HttpGet]
        public async Task<ActionResult> MyOrderDelete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orderEntity = OrderManager.Get(id);
            if (orderEntity == null)
            {
                return HttpNotFound();
            }
            return View("MyOrderDelete", (object)id);
        }


        /// <summary>
        /// POST: После подтверждения удаляет заказанный товар клиентом
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Список заказанных товаров клиента</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MyOrderDeleteConfirmed(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var orderEntity = OrderManager.Get(id);
            if (orderEntity == null)
            {
                return HttpNotFound();
            }

            OrderManager.Remove(id);

            return RedirectToAction("MyOrdersList", "User");
        }


        /// <summary>
        /// GET: Добавляет продукт
        /// </summary>
        /// <param name="order">Заказ</param>
        /// <param name="id">Идентификатор</param>
        /// <returns>Список заказанных товаров клиента</returns>
        [Authorize(Roles = Core.Roles.Employee)]
        public ActionResult OrderProductListAddProduct(int order, int id)
        {
            var orderEntity = OrderManager.Get(order);

            var product = ProductManager.Get(id);
            if (product == null)
                return RedirectToAction("ProductList", "Employee");

            orderEntity.Add(product, 1);

            OrderManager.Save(orderEntity);

            return Json("Товар добавлен в список", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GET: Удаляет продукт
        /// </summary>
        /// <param name="order">Заказ</param>
        /// <param name="id">Идентификатор</param>
        /// <returns>Список заказанных товаров клиента</returns>
        [Authorize(Roles = Core.Roles.Employee)]
        public ActionResult OrderProductListRemoveProduct(int order, int id)
        {
            var orderEntity = OrderManager.Get(order);

            var product = ProductManager.Get(id);
            if (product == null)
                return RedirectToAction("ProductList", "Employee");

            orderEntity.Remove(product, 1);

            OrderManager.Save(orderEntity);

            return Json("Товар удалён из списка", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Формирует список заказанных товаров клиента
        /// </summary>
        /// <param name="order">Заказ</param>
        /// <returns>Список заказанных товаров клиента</returns>
        public ActionResult OrderProductList(int order)
        {
            var orderEntity = OrderManager.Get(order);
            var model = new OrderProductListModel(orderEntity);

            return View(model);
        }
    }
}
