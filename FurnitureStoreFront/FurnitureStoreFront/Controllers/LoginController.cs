using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FurnitureStoreFront.Controllers
{
    public class LoginController : Controller
    {
        public Models.StoreFront.StoreFront StoreFront = new Models.StoreFront.StoreFront();


        // GET: Login
        public ActionResult Index()
        {
            return View(StoreFront);
        }

        public ActionResult Register()
        {
            if (HttpContext.Request.RequestType == "POST")
            {
                string Password = Request.Form["RPassword"];

                if (Models.User.Customer.InitialPasswordCheck(Password))
                {
                    string salt = Models.User.Customer.CreateSalt(8);
                    string HashedPassword = Models.User.Customer.GenerateSHA256Hash(Password, salt);
                    string fname = Request.Form["fName"];
                    string lname = Request.Form["lName"];
                    string Email = Request.Form["REmail"];

                   Models.StoreFront.StoreFront.CustomerList.Add(new Models.User.Customer(Models.StoreFront.StoreFront.CustomerList.Count + 1, fname, lname, Email, HashedPassword, salt));
                    Session["UserId"] = (Models.StoreFront.StoreFront.CustomerList.Count);
                    Models.Files.WorkingWithJSON<Models.User.Customer>.SaveData(Models.StoreFront.StoreFront.CustomerList, 2);

                    //Create cart JSON
                    Models.Files.WorkingWithJSON<Models.User.Customer>.CreateJSON(Models.StoreFront.StoreFront.CustomerList.Count);
                    //Create Receipt JSON
                    Models.Files.WorkingWithJSON<Models.User.Customer>.CreateReceiptJSON(Models.StoreFront.StoreFront.CustomerList.Count);

                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Login()
        {
            if (HttpContext.Request.RequestType == "POST")
            {
                var Email = Request.Form["LEmail"];
                var Password = Request.Form["LPassword"];

                var CurrentUser = Models.User.Customer.getCustomerData(Email, Models.StoreFront.StoreFront.CustomerList);

                if (CurrentUser != null)
                {
                    if (Models.User.Customer.VerifyHashedPassword(Password, CurrentUser.GetHashedPass(), CurrentUser.GetSalt()))
                    {

                        Session["UserId"] = CurrentUser.id;
                        StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)(Session["UserId"]), 0);
                        Session["Count"] = StoreFront.CustomerCart.Count;
                        Models.Files.WorkingWithJSON<Models.User.Customer>.SaveData(Models.StoreFront.StoreFront.CustomerList, 2);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            Models.Files.WorkingWithJSON<Models.User.Customer>.SaveData(Models.StoreFront.StoreFront.CustomerList,2);
            return RedirectToAction("Index","Login");
        }

        //public ActionResult Login()
        //{
        //}
    }
}