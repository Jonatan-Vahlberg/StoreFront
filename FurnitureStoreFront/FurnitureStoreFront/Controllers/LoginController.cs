using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FurnitureStoreFront.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// The storefront in its entierty
        /// </summary>
        /// BONUS COMMENT: if i remade the system i would have only inported the Userlist
        public Models.StoreFront.StoreFront StoreFront = new Models.StoreFront.StoreFront();


        // GET: Login
        public ActionResult Index()
        {
            return View(StoreFront);
        }
        /// <summary>
        /// Trying to register a user who has filled in the nececary form
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            //if this was accesed from a form
            if (HttpContext.Request.RequestType == "POST")
            {
                //Takes in password first if it does not match given criteria
                string Password = Request.Form["RPassword"];
                //Password is 8 chars long one uppercase and atleast one number
                if (Models.User.Customer.InitialPasswordCheck(Password))
                {
                    //Salt that will be saved along with hashed password
                    string salt = Models.User.Customer.CreateSalt(8);
                    //A Hash and salt of a password
                    string HashedPassword = Models.User.Customer.GenerateSHA256Hash(Password, salt);
                    //Firstname lastname email inputted
                    string fname = Request.Form["fName"];
                    string lname = Request.Form["lName"];
                    string Email = Request.Form["REmail"];

                    //Adds the new user to List
                    Models.StoreFront.StoreFront.CustomerList.Add(new Models.User.Customer(Models.StoreFront.StoreFront.CustomerList.Count + 1, fname, lname, Email, HashedPassword, salt));
                    
                    //Creates a Session variable for the user id
                    Session["UserId"] = (Models.StoreFront.StoreFront.CustomerList.Count);

                    //Writes updated list to JSON file
                    Models.Files.WorkingWithJSON<Models.User.Customer>.SaveData(Models.StoreFront.StoreFront.CustomerList, 2);

                    //Create cart JSON
                    Models.Files.WorkingWithJSON<Models.User.Customer>.CreateJSON(Models.StoreFront.StoreFront.CustomerList.Count);
                    //Create Receipt JSON
                    Models.Files.WorkingWithJSON<Models.User.Customer>.CreateReceiptJSON(Models.StoreFront.StoreFront.CustomerList.Count);

                    //going to home view
                    return RedirectToAction("Index", "Home");
                }
            }
            //going back to Login view
            return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Logs user in if password and 
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            //If the method was gotten trough POST METHOD
            if (HttpContext.Request.RequestType == "POST")
            {
                //Email to be verified
                var Email = Request.Form["LEmail"];

                //Password before rehashing and salting
                var Password = Request.Form["LPassword"];

                //A User found trough their email if multiple account with same emails has been registerd finds first one
                var CurrentUser = Models.User.Customer.getCustomerData(Email, Models.StoreFront.StoreFront.CustomerList);

                if (CurrentUser != null)
                {
                    //FINAL CHECK: Verifys the password of Current User THIS is complicated and built upon a outside library for obvious reasons
                    if (Models.User.Customer.VerifyHashedPassword(Password, CurrentUser.GetHashedPass(), CurrentUser.GetSalt()))
                    {
                        //Sets the session variable fur the user id
                        Session["UserId"] = CurrentUser.id;
                        
                        //Gets cart data to output a correct cart number
                        StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)(Session["UserId"]), 0);

                        //Sets the session variable fur the user Cart count
                        Session["Count"] = StoreFront.CustomerCart.Count;

                        //Saves the data for safety
                        Models.Files.WorkingWithJSON<Models.User.Customer>.SaveData(Models.StoreFront.StoreFront.CustomerList, 2);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            //Saves the data for safety
            Models.Files.WorkingWithJSON<Models.User.Customer>.SaveData(Models.StoreFront.StoreFront.CustomerList,2);
            return RedirectToAction("Index","Login");
        }

        
    }
}