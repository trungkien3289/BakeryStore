using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Model.Context;
using OnlineStore.Service.Interfaces;
using PagedList;
using OnlineStore.Service.Implements;
using OnlineStore.Model.ViewModel;
using OnlineStore.Infractructure.Utility;
using OnlineStore.Infractructure.Helper;
using OnlineStore.Model.MessageModel;
using System.Globalization;

namespace OnlineStoreMVC.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        #region Properties

        private IOrderManagementService orderService;

        #endregion 

        public OrderController()
        {
            orderService = new OrderManagementService();
        }

        #region Private Functions

        /// <summary>
        /// Get list options for Status dropdownlist and assign to Variable in ViewBag of view
        /// </summary>
        /// <param name="status"></param>
        private IEnumerable<SelectListItem> PopulateStatusDropDownList(Define.Status status = Define.Status.Active)
        {
            IEnumerable<Define.Status> values = Enum.GetValues(typeof(Define.Status)).Cast<Define.Status>();
            IEnumerable<SelectListItem> items = from value in values
                                                where value != Define.Status.Delete
                                                select new SelectListItem
                                                {
                                                    Text = EnumHelper.GetDescriptionFromEnum((Define.Status)value),
                                                    Value = ((int)value).ToString(),
                                                    Selected = value == status,
                                                };

            return items;
        }

        /// <summary>
        /// Get list options for Status dropdownlist and assign to Variable in ViewBag of view
        /// </summary>
        /// <param name="status"></param>
        private IEnumerable<SelectListItem> PopulateOrderStatusDropDownList(OrderStatus status = OrderStatus.NotDeliver)
        {
            IEnumerable<OrderStatus> values = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem
                                                {
                                                    Text = EnumHelper.GetDescriptionFromEnum((OrderStatus)value),
                                                    Value = ((int)value).ToString(),
                                                    Selected = value == status,
                                                };

            return items;
        }

        #endregion

        #region Actions
        // GET: Admin/Order
        public ActionResult Index(string keyword, int page = 1, string startDate = null, string endDate = null)
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
                toDate = outEndDate;
            }

            int totalItems = 0;
            var orders = orderService.GetOrders(page, OnlineStore.Infractructure.Utility.Define.PAGE_SIZE, fromDate, toDate, out totalItems);
            IPagedList<ShortSummaryOrderModel> pageProducts = new StaticPagedList<ShortSummaryOrderModel>(orders, page, OnlineStore.Infractructure.Utility.Define.PAGE_SIZE, totalItems);
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            return View(pageProducts);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = orderService.Delete(id);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OrderSumaryModel order = orderService.GetOrderDetails((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Admin/Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UpdateOrderModel order = orderService.getOrderForEdit((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }

            ViewBag.Status = PopulateStatusDropDownList((Define.Status)order.Status);
            ViewBag.OrderStatus = PopulateOrderStatusDropDownList((OrderStatus)order.OrderStatus);
            return View(order);
        }

        // POST: Admin/Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateOrderModel orderRequest)
        {
            if (ModelState.IsValid)
            {
                CultureInfo culture = CultureInfo.CreateSpecificCulture("vi-VN");
                DateTimeStyles styles = DateTimeStyles.None;
                DateTime outDeliveryDate;
                if (!DateTime.TryParse(orderRequest.DeliveryDate, culture, styles, out outDeliveryDate))
                {
                    return View(orderRequest);
                }

                ecom_Orders updatedOrder = new ecom_Orders()
                {
                    Id = orderRequest.Id,
                    FeeShip = orderRequest.FeeShip,
                    AddressOfRecipient = orderRequest.AddressOfRecipient,
                    NameOfRecipient = orderRequest.NameOfRecipient,
                    OrderNote = orderRequest.OrderNote,
                    PhoneOfRecipient = orderRequest.PhoneOfRecipient,
                    Status = orderRequest.Status,
                    OrderStatus = orderRequest.OrderStatus,
                    DeliveryDate = outDeliveryDate
                };

                bool result = orderService.UpdateOrder(updatedOrder);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(orderRequest);
                }
            }
            return View(orderRequest);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                orderService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
