using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FurnitureStoreFront.Models.Cart
{
    public class Receipt
    {
        public string Date { get; set; }
        public double FinalPrice { get; set; }
        public string FullName { get; set; }
        public string AccountNr { get; set; }
        
    }
}