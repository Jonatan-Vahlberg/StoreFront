using System;
using System.Collections.Generic;

namespace FurnitureStoreFront.Models.Cart
{
    public class Cart
    {
        
        public string finalPrice { get; set; }
        List<CartItem> CartItems = new List<CartItem>();
    }

    public class CartItem
    {
        public StoreItem.Furniture StoreItem;
        public int AmountofItem { get; set; }

        public CartItem(StoreItem.Furniture storeItem)
        {
            this.StoreItem = storeItem;
        }
    }
}