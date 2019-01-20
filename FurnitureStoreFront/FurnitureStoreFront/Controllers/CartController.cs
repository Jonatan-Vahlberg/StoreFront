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
            if(Session["UserId"] == null || (int)Session["UserId"] == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)(Session["UserId"]));
            Models.Cart.CartItem.SetFinalPrice(StoreFront.CustomerCart);
            return View(StoreFront);
        }

        public ActionResult ATC(string Page,int index)
        {
            if(Session["UserId"] != null)
            {

                StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)(Session["UserId"]));
                if (StoreFront.CustomerCart == null)
                {
                    StoreFront.CustomerCart = new List<Models.Cart.CartItem>();
                }

                var item = StoreFront.StoreStock.Find(x => x.id == index);
                Models.Cart.CartItem cartItem = new Models.Cart.CartItem((int)item.id, item.Name, item.Price, 1,item.ImageLink);
                Models.Cart.CartItem.AddToCart(cartItem, StoreFront.CustomerCart);
                
                Models.Files.WorkingWithJSON<Models.Cart.CartItem>.SaveCartData(StoreFront.CustomerCart, (int)(Session["UserId"]));
            }
            return RedirectToAction("Index", Page);
        }

        public ActionResult QuantityChanged(char operand,int id)
        {
            StoreFront.CustomerCart = Models.Files.WorkingWithJSON<Models.Cart.CartItem>.GetCartData((int)(Session["UserId"]));
            if (operand.Equals('P'))
            {
                StoreFront.CustomerCart.Find(x => x.Id == id).Quantity += 1;
            }
            else if (operand.Equals('-')){

                if (StoreFront.CustomerCart.Find(x => x.Id == id).Quantity < 2)
                {
                    StoreFront.CustomerCart.Remove(StoreFront.CustomerCart.Find(x => x.Id == id));
                }
                else
                { 
                    StoreFront.CustomerCart.Find(x => x.Id == id).Quantity -= 1;
                }

            }
            Models.Files.WorkingWithJSON<Models.Cart.CartItem>.SaveCartData(StoreFront.CustomerCart, (int)(Session["UserId"]));
            return RedirectToAction("index");
        }
    }
}