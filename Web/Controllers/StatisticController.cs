using System.Web;
using System.Web.Mvc;
using Core;
using Core.Managers;
using Microsoft.AspNet.Identity.Owin;

namespace Web.Controllers
{
    /// <summary>
    /// Контроллер статистики 
    /// </summary>
    [Authorize(Roles = Roles.Admin)]
    public class StatisticController : Controller
    {
        /// <summary>
        /// Поле менеджера для статистики
        /// </summary>
        private StatisticManager _statistictManager;

        /// <summary>
        /// Свойство менеджера для статистики
        /// </summary>
        public StatisticManager StatisticManager
        {
            get { return _statistictManager ?? HttpContext.GetOwinContext().Get<StatisticManager>(); }
            private set { _statistictManager = value; }
        }
    }
}