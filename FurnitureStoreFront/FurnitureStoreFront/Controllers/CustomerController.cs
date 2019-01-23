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
            //as long as user is logged in display page
            if(Session["UserId"] != null )
            {
                //Getting the user primed from Userlist
                var user = Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1];

                //Makes sure the purchaes get taken into account
                user.previousPurchases = Models.Files.WorkingWithJSON<Models.Cart.Receipt>.GetCartData((int)Session["UserId"], 1);
                return View(user);
            }
            //else go back
            return RedirectToAction("Index","Home");
        }
    }
}