using OnlineStore.Infractructure.Utility;
using OnlineStore.Service.Implements;
using OnlineStore.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStoreMVC.Controllers
{
    public class HomeController : BaseController
    {
        #region Properties

        IBannerService _bannerService = new BannerService();
        ICMSNewsService _cmsNewsService = new CMSNewsService();

        #endregion

        #region Constructures

        public HomeController()
        {
            _bannerService = new BannerService();
            _cmsNewsService = new CMSNewsService();
        }

        #endregion

        #region Private functions

        private IEnumerable<OnlineStore.Model.ViewModel.ProductSummaryView> GetTopProduct(MainProductCategory mainCategoryId,int top)
        {
            try{
                return service.GetTopProductsByCategoryId((int)mainCategoryId, top);
            }catch(Exception ex){
                return null;
            }
        }

        #endregion

        #region Actions

        public ActionResult Index()
        {
            PopulateNewProductList();
            PopulateBestSellProductList();
           
            //PopulateHighPriorityOrderProductList();
            //PopulateCategoryList();
            //PopulateTopCategoryList();

            // Populate list products by main categories

            ViewBag.ListBakeryProducts = GetTopProduct(MainProductCategory.BakeryProducts, 10);
            ViewBag.ListKitChenTools = GetTopProduct(MainProductCategory.KitchenTools, 10);

            //ViewBag.Banner2 = _bannerService.GetBanners2ForHomePage();
            //ViewBag.BannerPopup = _bannerService.GetActivePopupForHomePage();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.ContentNews = _cmsNewsService.GetCMSNewsById(OnlineStore.Infractructure.Utility.Define.ID_PAGE_INTRODUCTION).ContentNews;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult _HeaderPartial()
        {
            PopulateCategoryList();
            PopulateTopCategoryList();
            ViewBag.BakeryCategoryList = GetChildrenCategories(8);
            ViewBag.KitchenToolsCategoryList = GetChildrenCategories(9);
            return PartialView();
        }

        public ActionResult BannerPartial()
        {
            return PartialView(_bannerService.GetBanners1ForHomePage());
        }

        public ActionResult BlogPartial()
        {
            return PartialView(_cmsNewsService.GetCMSNewsForHomePage());
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
        }

        #endregion
    }
}