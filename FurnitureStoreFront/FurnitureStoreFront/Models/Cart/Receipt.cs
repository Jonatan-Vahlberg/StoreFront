using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FurnitureStoreFront.Models.Cart
{
    public class Receipt
    {
        /// <summary>
        /// Date of Purchase from Datetime variable
        /// </summary>
        public string Date { get; set; }
        
        /// <summary>
        /// Price of all items purchased
        /// </summary>
        public double FinalPrice { get; set; }

        /// <summary>
        /// A Name for Purchaser both last and first name
        /// </summary>
        public string FullName { get; set; }
        
        /// <summary>
        /// Number of Account the person used in purchase
        /// </summary>
        public string AccountNr { get; set; }
        
    }
}