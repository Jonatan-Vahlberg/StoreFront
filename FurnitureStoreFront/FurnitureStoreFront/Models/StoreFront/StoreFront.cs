using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FurnitureStoreFront.Models;

namespace FurnitureStoreFront.Models.StoreFront
{

    

    public class StoreFront
    {
        /// <summary>
        /// List of all customers dragged from JSON file
        /// </summary>
        public static List<User.Customer> CustomerList = User.Customer.getUsers();
        /// <summary>
        /// List of all Store items dragged from JSON file
        /// </summary>
        public static List<StoreItem.Furniture> StoreStock = new List<StoreItem.Furniture>(); 
    }
}