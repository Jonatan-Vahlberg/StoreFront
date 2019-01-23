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

        /// <summary>
        /// The storefront in its entierty
        /// </summary>
        /// BONUS COMMENT: if i remade the system i would have only inported the Userlist
        public Models.StoreFront.StoreFront StoreFront = new Models.StoreFront.StoreFront();

        // GET: Home
        public ActionResult Index()
        {
            //Creates instance of tempdata "FILTER"
            TempData["Filter"] = "";
            //Session["USERID"] = 1;

            //Orders the Total purchases by decending order
            StoreFront.StoreStock = StoreFront.OrderByTotalPurchases();
            return View(StoreFront);
        }

        /// <summary>
        /// Destroying all session variables accsesed by Logging Out of the Store
        /// </summary>
        /// <returns></returns>
        public ActionResult Destroy()
        {
            //As Long as User is loged in Abandon Session
            if (Session["UserId"] != null) Session.Abandon();

            //reorder the list for unregisterd user
            StoreFront.StoreStock = StoreFront.OrderByTotalPurchases();
            return View("Index", StoreFront);
        }
    }
}