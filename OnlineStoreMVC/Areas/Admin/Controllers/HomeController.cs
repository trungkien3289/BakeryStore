using OnlineStore.Service.Implements;
using OnlineStore.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStoreMVC.Areas.Admin.Controllers
{
    public class HomeController : BaseManagementController
    {
        #region Properties

        private IMenuService _menuService;

        #endregion

        public HomeController()
        {
            _menuService = new MenuService();
        }

        #region Actions
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var menu = _menuService.GetMenuByType((int)OnlineStore.Infractructure.Utility.Define.MenuEnum.Admin);
            return PartialView(menu);
        }

        #endregion

        #region Release resources

        /// <summary>
        /// Dispose database connection
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _menuService.Dispose();
        }

        #endregion
    }
}
