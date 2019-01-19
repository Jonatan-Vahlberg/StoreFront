using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FurnitureStoreFront.Models;


namespace FurnitureStoreFront.Controllers
{
    public class HomeController : Controller
    {
        public Models.StoreFront.StoreFront StoreFront = new Models.StoreFront.StoreFront();
        // GET: Home
        public ActionResult Index()
        {
            TempData["Filter"] = "";
            //Session["USERID"] = 1;
            StoreFront.StoreStock = StoreFront.OrderByTotalPurchases();
            return View(StoreFront);
        }
    }
}