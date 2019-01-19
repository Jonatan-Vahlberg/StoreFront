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

                    StoreFront.CustomerList.Add(new Models.User.Customer(StoreFront.CustomerList.Count + 1, fname, lname, Email, HashedPassword, salt));
                    Session["UserId"] = (StoreFront.CustomerList.Count);
                    Models.Files.WorkingWithJSON<Models.User.Customer>.SaveData(StoreFront.CustomerList, 2);
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

                var CurrentUser = Models.User.Customer.getCustomerData(Email,StoreFront.CustomerList);

                if (CurrentUser != null)
                {
                    if (Models.User.Customer.VerifyHashedPassword(Password, CurrentUser.GetHashedPass(), CurrentUser.GetSalt()))
                    {

                        Session["UserId"] = CurrentUser.id;
                        Models.Files.WorkingWithJSON<Models.User.Customer>.SaveData(StoreFront.CustomerList, 2);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            Models.Files.WorkingWithJSON<Models.User.Customer>.SaveData(StoreFront.CustomerList,2);
            return RedirectToAction("Index","Login");
        }

        //public ActionResult Login()
        //{
        //}
    }
}