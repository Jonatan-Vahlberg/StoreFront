using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FurnitureStoreFront.Controllers
{
    public class CartController : Controller
    {
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

        public ActionResult ATC(string Page,int index)
        {
            if(Session["UserId"] != null)
            {

                StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)(Session["UserId"]),0);
                if (StoreFront.CustomerCart == null)
                {
                    StoreFront.CustomerCart = new List<Models.Cart.CartItem>();
                }
                
                Models.StoreItem.Furniture item = StoreFront.StoreStock.Find(x => x.id == index);
                Models.Cart.CartItem cartItem = new Models.Cart.CartItem((int)item.id, item.Name, item.Price, 1, item.ImageLink, item.Room)
                {
                    Stock = item.Stock
                };
                Models.Cart.CartItem.AddToCart(cartItem, StoreFront.CustomerCart);
                Session["Count"] = StoreFront.CustomerCart.Count;
                Models.Files.WorkingWithJSON<Models.Cart.CartItem>.SaveCartData(StoreFront.CustomerCart, (int)(Session["UserId"]));
            }
            return RedirectToAction("Index", Page);
        }

        public ActionResult QuantityChanged(char operand,int id)
        {
            StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)(Session["UserId"]),0);
            if (operand.Equals('P'))
            {
                StoreFront.CustomerCart.Find(x => x.Id == id).Quantity += 1;
            }
            else if (operand.Equals('-')){

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
                else
                { 
                    StoreFront.CustomerCart.Find(x => x.Id == id).Quantity -= 1;
                }

            }
            Models.Files.WorkingWithJSON<Models.Cart.CartItem>.SaveCartData(StoreFront.CustomerCart, (int)(Session["UserId"]));
            return RedirectToAction("index");
        }

        public ActionResult Checkout()
        {

            return View(StoreFront);
        }

        public ActionResult AddToAccount()
        {
            StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)(Session["UserId"]),0);
            Models.Cart.CartItem.SetFinalPrice(StoreFront.CustomerCart);

            if (HttpContext.Request.RequestType == "POST")
            {
                string fname = Request.Form["Firstname"];
                string lname = Request.Form["Lastname"];
                string Month = Request.Form["Month"];
                string Year = Request.Form["Month"];
                string Nr = Request.Form["AccNr"];
                string CVC = Request.Form["CVCNr"];

                int lengthOfAccNr = Nr.Length;
                string accNrRedacted = string.Empty;
                for (int i = 0; i < lengthOfAccNr; i++)
                {
                    accNrRedacted += "*";
                }
                accNrRedacted = accNrRedacted.Remove(accNrRedacted.Length - 4, 4) + Nr.Substring(Nr.Length - 4);
                Models.Cart.Receipt receipt = new Models.Cart.Receipt()
                {
                    FullName = fname + " " + lname,
                    Date = DateTime.Now.ToString(),
                    AccountNr = accNrRedacted,
                    FinalPrice = Models.Cart.CartItem.FinalPrice

                };
                Models.StoreFront.StoreFront.CustomerList = Models.Files.WorkingWithJSON<Models.User.Customer>.GetData(2);
                Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases = Models.Files.WorkingWithJSON<Models.Cart.Receipt>.GetCartData((int)Session["UserId"], 1);
                if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases == null) Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases = new List<Models.Cart.Receipt>();
                Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]-1].previousPurchases.Add(receipt);
                Models.Files.WorkingWithJSON<Models.Cart.Receipt>.SaveCartData(Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]-1].previousPurchases, (int)Session["UserId"], 1);
            }
            else
            {
                Models.Cart.Receipt receipt = new Models.Cart.Receipt()
                {
                    FullName = Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]-1].Firstname + " " + Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]-1].Lastname,
                    Date = DateTime.Now.ToString(),
                    AccountNr = "[REDACTED]",
                    FinalPrice = Models.Cart.CartItem.FinalPrice

                };

                Models.StoreFront.StoreFront.CustomerList = Models.Files.WorkingWithJSON<Models.User.Customer>.GetData(2);
                if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases == null)
                { 
                    Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases = Models.Files.WorkingWithJSON<Models.Cart.Receipt>.GetCartData((int)Session["UserId"], 1);
                    if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases == null) Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases = new List<Models.Cart.Receipt>();
                }
                Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases.Add(receipt);
                Models.Files.WorkingWithJSON<Models.Cart.Receipt>.SaveCartData(Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].previousPurchases, (int)Session["UserId"], 1);
            }

            if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics == null)
            {
                Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics = Models.Files.WorkingWithJSON<Models.User.Customer>.GetPersonalData((int)Session["UserId"]);

                if (Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics == null)  Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics = Models.User.Customer.FillStatistics();
            }
            foreach (var item in StoreFront.CustomerCart)
            {
                //Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]-1].PersonalStatisics = Models.User.Customer.AddToPersonalStatistics(Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"]].PersonalStatisics, item.Id, StoreFront.StoreStock);
                Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics[item.Room] += 1;
                StoreFront.StoreStock.Find(x => x.id == item.Id).SetTotalPurchases(true, (uint)item.Quantity);
                StoreFront.StoreStock.Find(x => x.id == item.Id).Stock -= item.Quantity;
            }
            Models.Files.WorkingWithJSON<Models.StoreItem.Furniture>.SaveData(StoreFront.StoreStock,1);
            Models.Files.WorkingWithJSON<Models.User.Customer>.SavePersonalData(Models.StoreFront.StoreFront.CustomerList[(int)Session["UserId"] - 1].PersonalStatisics,(int)Session["UserId"]);

            Models.Files.WorkingWithJSON<Models.User.Customer>.SaveData(Models.StoreFront.StoreFront.CustomerList,2);
            Models.Files.WorkingWithJSON<Models.Cart.CartItem>.SaveCartData(new List<Models.Cart.CartItem>(), (int)Session["UserId"],0);

            return RedirectToAction("Index", "Home");
        }
    }
}