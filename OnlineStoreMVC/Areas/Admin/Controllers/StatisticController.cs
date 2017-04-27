using OnlineStore.Model.MessageModel;
using OnlineStore.Service.Implements;
using OnlineStore.Service.Interfaces;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStoreMVC.Areas.Admin.Controllers
{
    public class StatisticController : Controller
    {
        private IOrderManagementService orderService;

        public StatisticController()
        {
            orderService = new OrderManagementService();
        }
        // GET: Admin/Statistic
        public ActionResult Index(int page = 1, string startDate = null, string endDate = null)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("vi-VN");
            DateTimeStyles styles = DateTimeStyles.None;
            DateTime outStartDate;
            DateTime outEndDate;
            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (DateTime.TryParse(startDate, culture, styles, out outStartDate))
            {
                fromDate = outStartDate;
            }
            if (DateTime.TryParse(endDate, culture, styles, out outEndDate))
            {
                toDate = outEndDate.AddDays(1);
            }
            int totalItems = 0;
            IList<SaleStatisticsModel> orderProducts = orderService.getSaleStatisticsAfterProduct(page, OnlineStore.Infractructure.Utility.Define.PAGE_SIZE, fromDate, toDate, out totalItems);
            //var orders = orderService.GetOrders(page, OnlineStore.Infractructure.Utility.Define.PAGE_SIZE, fromDate, toDate, out totalItems);
            IPagedList<SaleStatisticsModel> products = new StaticPagedList<SaleStatisticsModel>(orderProducts, page, OnlineStore.Infractructure.Utility.Define.PAGE_SIZE, totalItems);
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;

            return View(products);
        }

        #region Release resources

        /// <summary>
        /// Dispose database connection
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            orderService.Dispose();
        }

        #endregion
    }
}