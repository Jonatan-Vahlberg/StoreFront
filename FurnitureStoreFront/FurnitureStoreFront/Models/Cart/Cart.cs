using System;
using System.Collections.Generic;
using FurnitureStoreFront.Models.StoreItem;

namespace FurnitureStoreFront.Models.Cart
{
    public class CartItem
    {
        /// <summary>
        /// A price of the total price
        /// </summary>
        public static double FinalPrice { get; set; }

        /// <summary>
        /// Id of Cartitem based on <see cref="Models.StoreItem.Furniture.id"/>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// name of Cartitem based on <see cref="Models.StoreItem.Furniture.name"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// price of Cartitem based on <see cref="Models.StoreItem.Furniture.Price"/>
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Quantity of Cartitem based on the amount of the item
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Imagelink of Cartitem based on <see cref="Models.StoreItem.Furniture.ImageLink"/>
        /// </summary>
        public string ImageLink { get; set; }

        /// <summary>
        /// Room of Cartitem based on <see cref="Models.StoreItem.Furniture.Room"/>
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// Stock of Cartitem based on <see cref="Models.StoreItem.Furniture.Stock"/>
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// constructor for Cartitem
        /// </summary>
        public CartItem(int id, string name,double price,int quantity,string imageLink,string room)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Quantity = quantity;
            this.ImageLink = imageLink;
            this.Room = room;
        }

        /// <summary>
        /// Adding a item to the cart
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Cart"></param>
        public static void AddToCart(CartItem item, List<CartItem> Cart)
        {
            //All current items in cart
            foreach(var ic in Cart)
            {
                //Checking the Name and if found adding to Quantity EX +1
                if(ic.Name == item.Name)
                {
                    ic.Quantity++;
                    //Call of the method to set a final price based on pricing as the foreach loop exits directly after
                    SetFinalPrice(Cart);
                    return;
                }
            }
            item.Quantity = 1;
            Cart.Add(item);
            
            SetFinalPrice(Cart);
        }

        /// <summary>
        /// Setting the final price based on the items in cart and their quantity
        /// </summary>
        /// <param name="cart"></param>
        public static void SetFinalPrice(List<CartItem> cart)
        {
            double price = 0;
            foreach( var item in cart)
            {
                price += (item.Price * item.Quantity);
            }
            FinalPrice = price;
        }
    }
}