using System;
using System.Collections.Generic;

namespace FurnitureStoreFront.Models.Cart
{
    public class Cart
    {

        public int finalPrice { get; set; } = 0;
        public Dictionary<StoreItem.Furniture, int> Cartitems = new Dictionary<StoreItem.Furniture, int>();
    }
}