using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FurnitureStoreFront.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {

            
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Login()
        {
            if (HttpContext.Request.RequestType == "POST")
            {
                var Email = Request.Form["LEmail"];
                var Password = Request.Form["LPassword"];

                //var CurrentUser = Models.User.Customer.getCustomerData(Email);

                //if (CurrentUser != null)
                //{
                //    if (Models.User.Customer.VerifyHashedPassword(Password, CurrentUser.GetHashedPass(), CurrentUser.GetSalt()))
                //    {

                //    Session["UserId"] = CurrentUser.id;
                //    return RedirectToAction("Index", "Home");
                //    }
                //}
            }
            return RedirectToAction("Index", "Login");
        }

        //public ActionResult Login()
        //{
        //}
    }
}