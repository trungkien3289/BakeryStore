using OnlineStore.Model.MessageModel;
using OnlineStore.Service.Implements;
using OnlineStore.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStoreMVC.Controllers
{
    public class OrderController : Controller
    {

        private IOrderManagementService orderService;

        public OrderController()
        {
            orderService = new OrderManagementService();
        }

        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(CheckOutModelRequest checkOutModel)
        {
            var createOrderRequest = new RequestCreateOrderModel()
            {
                NameOfRecipient = checkOutModel.FullName,
                PhoneOfRecipient = checkOutModel.PhoneNumber,
                AddressOfRecipient = checkOutModel.Address,
                FeeShip = 0,
                Products = checkOutModel.Products.ToList(),
                OrderNote = checkOutModel.Note !=null ?checkOutModel.Note:String.Empty
            };
            bool result = orderService.CreateOrder(createOrderRequest);

            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckOut()
        {
            return View();
        }

        public ActionResult OrderSuccessful()
        {
            return View();
        }
    }
}