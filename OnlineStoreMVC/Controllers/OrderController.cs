using OnlineStore.Model.MessageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStoreMVC.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(CheckOutModelRequest checkOutModel)
        {
            Console.WriteLine("abc");
            return Json(new {success = true},JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckOut()
        {
            return View();
        }
    }
}