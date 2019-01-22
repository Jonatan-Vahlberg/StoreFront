using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FurnitureStoreFront.Controllers
{
    public class CustomerController : Controller
    {
        
        // GET: Customer
        public ActionResult Index()

        {
            if(Session["UserId"] != null )
            {
                var user = Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1];
                user.previousPurchases = Models.Files.WorkingWithJSON<Models.Cart.Receipt>.GetCartData((int)Session["UserId"], 1);
                return View(user);
            }
            return RedirectToAction("Index","Home");
        }
    }
}