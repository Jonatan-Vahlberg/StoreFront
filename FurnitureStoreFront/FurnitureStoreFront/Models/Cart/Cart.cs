using System;
using System.Collections.Generic;
using FurnitureStoreFront.Models.StoreItem;

namespace FurnitureStoreFront.Models.Cart
{
    public class CartItem
    {
        public static double FinalPrice { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImageLink { get; set; }

        public CartItem(int id, string name,double price,int quantity,string imageLink)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Quantity = quantity;
            this.ImageLink = imageLink;
        }

        public static void AddToCart(CartItem item, List<CartItem> Cart)
        {
            foreach(var ic in Cart)
            {
                if(ic.Name == item.Name)
                {
                    ic.Quantity++;
                    SetFinalPrice(Cart);
                    return;
                }
            }
            item.Quantity = 1;
            Cart.Add(item);
            
            SetFinalPrice(Cart);
        }

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