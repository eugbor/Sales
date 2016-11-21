using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Managers;
using Core.Entities;
using Microsoft.AspNet.Identity.Owin;
using Web.Models.ProductViews;

namespace Web.Controllers
{
    /// <summary>
    /// Контроллер главной страницы
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Поле менеджера для товара
        /// </summary>
        private ProductManager _productManager;

        /// <summary>
        /// Свойство менеджера для товара
        /// </summary>
        public ProductManager ProductManager
        {
            get { return _productManager ?? HttpContext.GetOwinContext().Get<ProductManager>(); }
            private set { _productManager = value; }
        }

        /// <summary>
        /// GET: Формирует список продуктов по цене
        /// </summary>
        /// <returns>Список продуктов</returns>
        public ActionResult Index(bool? isAcsending, int? min, int? max)
        {
            List<Product> products = new List<Product>();
            if (isAcsending == null)
            {
                products = ProductManager.GetList(min, max, null);
            }
            else if(isAcsending == true)
            {
                products = ProductManager.GetList(min, max, null).OrderBy(s => s.Cost).ToList();
            }
            else
            {
                products = ProductManager.GetList(min, max, null).OrderByDescending(s => s.Cost).ToList();
            }
            var model = new ProductHomeListModel(products);

            return View(model);
        }
    }
}