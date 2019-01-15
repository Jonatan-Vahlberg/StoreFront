using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FurnitureStoreFront.Models.Cart
{
    public class Receipt
    {
        public string Date { get; set; }
        public string FinalPrice { get; set; }
        List<string> Items = new List<string>();

    }
}