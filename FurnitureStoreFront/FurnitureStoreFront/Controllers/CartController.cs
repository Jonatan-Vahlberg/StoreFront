using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FurnitureStoreFront.Models.Cart;

namespace FurnitureStoreFront.Controllers
{
    public class CartController : Controller
    {

        /// <summary>
        /// Storefront that is put into view
        /// </summary>
        public Models.StoreFront.StoreFront StoreFront = new Models.StoreFront.StoreFront();
        

        // GET: Cart
        public ActionResult Index()
        {   
            //Check the session varible to make sure its a user
            if(Session["UserId"] == null || (int)Session["UserId"] == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            //load in the customer Cart
            StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)(Session["UserId"]),0);
            //set the final price of current cart
            Models.Cart.CartItem.SetFinalPrice(StoreFront.CustomerCart);
            return View(StoreFront);
        }

        /// <summary>
        /// Add to Cart method
        /// </summary>
        /// <param name="Page">page you are coming from</param>
        /// <param name="index">index of item to be added</param>
        /// <returns></returns>
        public ActionResult ATC(string Page,int index)
        {
            //If user is logged in otherwise no adding
            if(Session["UserId"] != null)
            {
                //Filling the cart from its JSON
                StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)(Session["UserId"]),0);

                //If that file was empty or did not exsist give a brand new customer cart
                if (StoreFront.CustomerCart == null)
                {
                    StoreFront.CustomerCart = new List<Models.Cart.CartItem>();
                }
                //Finds item based on its id  and if it matches the index of the item sougt after
                Models.StoreItem.Furniture item = StoreFront.StoreStock.Find(x => x.id == index);
                
                //Makes furniture object into Cartitems
                Models.Cart.CartItem cartItem = new Models.Cart.CartItem((int)item.id, item.Name, item.Price, 1, item.ImageLink, item.Room)
                {
                    //Adds the stock since it is not part of the constructor
                    Stock = item.Stock
                };

                //Atempt to put item in cart
                Models.Cart.CartItem.AddToCart(cartItem, StoreFront.CustomerCart);
                
                //create or change the Session variable used later
                Session["Count"] = StoreFront.CustomerCart.Count;

                //Saves new data
                Models.Files.WorkingWithJSON<Models.Cart.CartItem>.SaveCartData(StoreFront.CustomerCart, (int)(Session["UserId"]));
            }
            return RedirectToAction("Index", Page);
        }

        /// <summary>
        /// Adding to item or removing an item from cart view
        /// </summary>
        /// <param name="operand">a P for plus or a - serves as the opperand in the equation</param>
        /// <param name="id">id of cartitem</param>
        /// <returns></returns>
        public ActionResult QuantityChanged(char operand,int id)
        {
            //getting the customers cart based on id
            StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)(Session["UserId"]),0);
            //Adding to item count
            if (operand.Equals('P'))
            {
                StoreFront.CustomerCart.Find(x => x.Id == id).Quantity += 1;
            }
            //taking from item count
            else if (operand.Equals('-')){

                //after taking from count it will not be in the cart
                if (StoreFront.CustomerCart.Find(x => x.Id == id).Quantity < 2)
                {
                    StoreFront.CustomerCart.Remove(StoreFront.CustomerCart.Find(x => x.Id == id));
                    if(StoreFront.CustomerCart.Count == 0)
                    {
                        Session["Count"] = 0;
                    }
                    else
                    {
                        Session["Count"] = StoreFront.CustomerCart;
                    }
                }
                //Normal removal
                else
                { 
                    StoreFront.CustomerCart.Find(x => x.Id == id).Quantity -= 1;
                }

            }
            //Saves the updated data
            Models.Files.WorkingWithJSON<Models.Cart.CartItem>.SaveCartData(StoreFront.CustomerCart, (int)(Session["UserId"]));
            
            return RedirectToAction("index");
        }

        /// <summary>
        /// Action to display checkout Modalbox
        /// </summary>
        /// <returns></returns>
        public ActionResult Checkout()
        {

            return View(StoreFront);
        }

        /// <summary>
        /// Add evrything to acount as a purchase
        /// </summary>
        /// <returns></returns>
        //public ActionResult AddToAccount()
        //{
        //    StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)(Session["UserId"]),0);
        //    Models.Cart.CartItem.SetFinalPrice(StoreFront.CustomerCart);

        //    if (HttpContext.Request.RequestType == "POST")
        //    {
        //        string fname = Request.Form["Firstname"];
        //        string lname = Request.Form["Lastname"];
        //        string Month = Request.Form["Month"];
        //        string Year = Request.Form["Month"];
        //        string Nr = Request.Form["AccNr"];
        //        string CVC = Request.Form["CVCNr"];

        //        int lengthOfAccNr = Nr.Length;
        //        string accNrRedacted = string.Empty;
        //        for (int i = 0; i < lengthOfAccNr; i++)
        //        {
        //            accNrRedacted += "*";
        //        }
        //        accNrRedacted = accNrRedacted.Remove(accNrRedacted.Length - 4, 4) + Nr.Substring(Nr.Length - 4);
        //        Models.Cart.Receipt receipt = new Models.Cart.Receipt()
        //        {
        //            FullName = fname + " " + lname,
        //            Date = DateTime.Now.ToString(),
        //            AccountNr = accNrRedacted,
        //            FinalPrice = Models.Cart.CartItem.FinalPrice

        //        };
        //        Models.StoreFront.StoreFront.CustomerList = Models.Files.WorkingWithJSON<Models.User.Customer>.GetData(2);
        //        Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases = Models.Files.WorkingWithJSON<Models.Cart.Receipt>.GetCartData((int)Session["UserId"], 1);
        //        if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases == null) Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases = new List<Models.Cart.Receipt>();
        //        Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]-1].previousPurchases.Add(receipt);
        //        Models.Files.WorkingWithJSON<Models.Cart.Receipt>.SaveCartData(Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]-1].previousPurchases, (int)Session["UserId"], 1);
        //    }
        //    else
        //    {
        //        Models.Cart.Receipt receipt = new Models.Cart.Receipt()
        //        {
        //            FullName = Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]-1].Firstname + " " + Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]-1].Lastname,
        //            Date = DateTime.Now.ToString(),
        //            AccountNr = "[REDACTED]",
        //            FinalPrice = Models.Cart.CartItem.FinalPrice

        //        };

        //        Models.StoreFront.StoreFront.CustomerList = Models.Files.WorkingWithJSON<Models.User.Customer>.GetData(2);
        //        if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases == null)
        //        { 
        //            Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases = Models.Files.WorkingWithJSON<Models.Cart.Receipt>.GetCartData((int)Session["UserId"], 1);
        //            if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases == null) Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases = new List<Models.Cart.Receipt>();
        //        }
        //        Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases.Add(receipt);
        //        Models.Files.WorkingWithJSON<Models.Cart.Receipt>.SaveCartData(Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases, (int)Session["UserId"], 1);
        //    }

        //    if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics == null)
        //    {
        //        Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics = Models.Files.WorkingWithJSON<Models.User.Customer>.GetPersonalData((int)Session["UserId"]);

        //        if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics == null)  Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics = Models.User.Customer.FillStatistics();
        //    }
        //    foreach (var item in StoreFront.CustomerCart)
        //    {
        //        //Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]-1].PersonalStatisics = Models.User.Customer.AddToPersonalStatistics(Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]].PersonalStatisics, item.Id, StoreFront.StoreStock);
        //        Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics[item.Room] += 1;
        //        StoreFront.StoreStock.Find(x => x.id == item.Id).SetTotalPurchases(true, (uint)item.Quantity);
        //        StoreFront.StoreStock.Find(x => x.id == item.Id).Stock -= item.Quantity;
        //    }
        //    Models.Files.WorkingWithJSON<Models.StoreItem.Furniture>.SaveData(StoreFront.StoreStock,1);

        //    Models.Files.WorkingWithJSON<Models.User.Customer>.SavePersonalData(Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics,(int)Session["UserId"]);

        //    Models.Files.WorkingWithJSON<Models.User.Customer>.SaveData(Models.StoreFront.StoreFront.CustomerList,2);
        //    Models.Files.WorkingWithJSON<Models.Cart.CartItem>.SaveCartData(new List<Models.Cart.CartItem>(), (int)Session["UserId"],0);

        //    return RedirectToAction("Index", "Home");
        //}

        /// <summary>
        /// Adds evrything to account as a purchaseed receipt
        /// This actionresult is a composite of multiple methods that aim
        /// to close of the purchase
        /// </summary>
        /// <returns></returns>
        public ActionResult FinishCheckout()
        {
            //dummy receipt item created
            Models.Cart.Receipt receipt;

            //If form was filled in
            if(HttpContext.Request.RequestType == "POST")
            {
                receipt = GetReceiptFromForm();
            }
            //else ie if klarna autofill was selected
            else
            {
                receipt = GetReceiptFromAutofill();
            }
            //Updates acount receipts
            UpdatePreviousPurchases(receipt);

            //Retrieves cartdata to overwrite
            StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)Session["UserId"], 0);

            //overwrites the stock and total purches
            UpdateStoreStock();

            //overwrites the statistics of the user
            UpdatePersonalStatistics();

            //saves all primary data
            Models.Files.WorkingWithJSON<Models.User.Customer>.SaveData(Models.StoreFront.StoreFront.CustomerList, 2);
            Models.Files.WorkingWithJSON<Models.Cart.CartItem>.SaveCartData(new List<Models.Cart.CartItem>(), (int)Session["UserId"], 0);

            //resets count variable
            Session["Count"] = 0;
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Sets the data based on predetrmind values
        /// </summary>
        /// <returns></returns>
        private Receipt GetReceiptFromAutofill()
        {
            Models.Cart.Receipt receipt = new Models.Cart.Receipt()
            {
                FullName = Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].Firstname + " " + Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].Lastname,
                Date = DateTime.Now.ToString(),
                AccountNr = "[REDACTED]",
                FinalPrice = Models.Cart.CartItem.FinalPrice

            };
            return receipt;
        }

        /// <summary>
        /// Gets the gatherd data from a receipt form
        /// </summary>
        /// <returns></returns>
        public Receipt GetReceiptFromForm()
        {
            string fname = Request.Form["Firstname"];
            string lname = Request.Form["Lastname"];
            string Month = Request.Form["Month"];
            string Year = Request.Form["Month"];
            string Nr = Request.Form["AccNr"];
            string CVC = Request.Form["CVCNr"];

            //gets length of account nr
            int lengthOfAccNr = Nr.Length;
            string accNrRedacted = string.Empty;

            //Foreach char in account nr replace with star char 
            for (int i = 0; i < lengthOfAccNr; i++)
            {
                accNrRedacted += "*";
            }
            if(lengthOfAccNr > 5)
            {

            //take last fourchars in redacted account number and return last four numbers in original account nr
            accNrRedacted = accNrRedacted.Remove(accNrRedacted.Length - 4, 4) + Nr.Substring(Nr.Length - 4);
            }

            //Create new receipt object
             Models.Cart.Receipt receipt = new Models.Cart.Receipt()
            {
                FullName = fname + " " + lname,
                Date = DateTime.Now.ToString(),
                AccountNr = accNrRedacted,
                FinalPrice = Models.Cart.CartItem.FinalPrice

            };
            return receipt;
        }

        /// <summary>
        /// Updates the receipts list of Customer
        /// </summary>
        /// <param name="receipt"></param>
        private void UpdatePreviousPurchases(Receipt receipt)
        {
            Models.StoreFront.StoreFront.CustomerList = Models.Files.WorkingWithJSON<Models.User.Customer>.GetData(2);

            //If the list is still null after first atempt of filling
            if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases == null)
            {
                //Fill with backup data
                Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases = Models.Files.WorkingWithJSON<Models.Cart.Receipt>.GetCartData((int)Session["UserId"], 1);
                //if still null empty
                if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases == null)
                    //Populate with empty list to avoid crashing
                    Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases = new List<Models.Cart.Receipt>();
            }
            //add to list
            Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases.Add(receipt);
            //Save data
            Models.Files.WorkingWithJSON<Models.Cart.Receipt>.SaveCartData(Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases, (int)Session["UserId"], 1);
        }
        
        /// <summary>
        /// Update the personal statistics of user
        /// </summary>
        private void UpdatePersonalStatistics()
        {
            //Checks for nullpoint refrence
            if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics == null)
            {
                //atempts to fill dictionary
                Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics = Models.Files.WorkingWithJSON<Models.User.Customer>.GetPersonalData((int)Session["UserId"]);

                //if still empty fill with base statistics
                if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics == null)
                    Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics = Models.User.Customer.FillStatistics();
            }
            //for each items fill list with its Quantity
            foreach (var item in StoreFront.CustomerCart)
            {
                Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics[item.Room] += item.Quantity;
            }

            //save the data
            Models.Files.WorkingWithJSON<Models.User.Customer>.SavePersonalData(Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics, (int)Session["UserId"]);

        }

        /// <summary>
        /// Updates all stock
        /// </summary>
        private void UpdateStoreStock()
        {
            //Adds to total purches and takes away from store stock
            foreach (var item in StoreFront.CustomerCart)
            {
                //Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]-1].PersonalStatisics = Models.User.Customer.AddToPersonalStatistics(Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]].PersonalStatisics, item.Id, StoreFront.StoreStock);
                
                StoreFront.StoreStock.Find(x => x.id == item.Id).SetTotalPurchases(true, (uint)item.Quantity);
                StoreFront.StoreStock.Find(x => x.id == item.Id).Stock -= item.Quantity;
            }
            Models.Files.WorkingWithJSON<Models.StoreItem.Furniture>.SaveData(StoreFront.StoreStock, 1);

        }
    }
}