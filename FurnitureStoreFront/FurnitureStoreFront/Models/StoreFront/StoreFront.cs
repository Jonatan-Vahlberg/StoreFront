﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FurnitureStoreFront.Models;
using FurnitureStoreFront.Models.Cart;

namespace FurnitureStoreFront.Models.StoreFront
{

    

    public class StoreFront
    {
        /// <summary>
        /// A string for filtering results in the catalogue
        /// </summary>
        public string Searchstring { get; set; }
        /// <summary>
        /// Apart of the filtering process
        /// </summary>
        public int SearchInt { get; set; }
        /// <summary>
        /// List of all customers dragged from JSON file
        /// </summary>
        public static List<User.Customer> CustomerList = Files.WorkingWithJSON<User.Customer>.GetData(2);


        /// <summary>
        /// Cart of the User Currently Logged in
        /// </summary>
        public List<Cart.CartItem> CustomerCart = new List<CartItem>();

        /// <summary>
        /// List of all Store items dragged from JSON file
        /// </summary>
        public List<StoreItem.Furniture> StoreStock = Files.WorkingWithJSON<StoreItem.Furniture>.GetData(1);

        /// <summary>
        /// Initial Method or backup if JSON file is destroyed for filling store stock to the base items
        /// </summary>
        /// <returns></returns>
        private static List<StoreItem.Furniture> FillStockList()
        {
            List<StoreItem.Furniture> StockList = new List<StoreItem.Furniture>();

            StockList.Add(new StoreItem.Chair("Black Kitchen Chair",(uint)(StockList.Count+1),125,"Kitchen",(StoreItem.Color)17,StoreItem.Material.Wood,1,0));
            StockList.Add(new StoreItem.Chair("Red Leather Armchair",(uint)(StockList.Count+1),870,"Livingroom",(StoreItem.Color)64,StoreItem.Material.Leather,1,0));
            StockList.Add(new StoreItem.Chair("Plastic Lawn chairs",(uint)(StockList.Count+1),45,"Outdoors",(StoreItem.Color)2,StoreItem.Material.Plastic,4,0));

            StockList.Add(new StoreItem.Table("Darkwood Writing Desk", (uint)(StockList.Count + 1), 768, "Bedroom", (StoreItem.Color)16, StoreItem.Material.Wood,2.5,0));
            StockList.Add(new StoreItem.Table("Beige Coffe Table", (uint)(StockList.Count + 1), 456, "Livingroom", (StoreItem.Color)4, StoreItem.Material.Wood, 0.8,0));
            StockList.Add(new StoreItem.Table("Plastic Lawn Table Two Piece", (uint)(StockList.Count + 1), 199, "Outdoors", (StoreItem.Color)2, StoreItem.Material.Plastic, 6.3,0));

            StockList.Add(new StoreItem.Bed("White Bedframe with cotton matress", (uint)(StockList.Count + 1), 3399, "Bedroom", (StoreItem.Color)18, (StoreItem.Material)17,90,210,StoreItem.BedType.Twinsizedbed,13));
            StockList.Add(new StoreItem.Bed("Black Leather Vintage Studiocouch", (uint)(StockList.Count + 1), 8999, "Livingroom", (StoreItem.Color)128, (StoreItem.Material)22,90,190, StoreItem.BedType.Studicouch,0));
            
            

            return StockList;
        }

        /// <summary>
        /// Orders the list based on the total amount of purchases
        /// first is the most purchased
        /// </summary>
        /// <returns></returns>
        public List<StoreItem.Furniture> OrderByTotalPurchases()
        {
            List<StoreItem.Furniture> newList = new List<StoreItem.Furniture>();
            //Result is the orderd list of StoreStock
            //Orderd in decending order so most popular first
            var result =
                from o in StoreStock
                orderby o.TotalPurchases descending
                select o;
            foreach (var item in result)
            {
                newList.Add(item);
            }
            return newList;
            
        }

        /// <summary>
        /// Filter <see cref="StoreStock"/> based on if there is stock of that give item
        /// </summary>
        /// <returns></returns>
        public List<StoreItem.Furniture> StockFilter()
        {
            //Finds all that hava a stock of more than 0 and returns a list of those items ALGORITHM
            return StoreStock = StoreStock.FindAll(x => x.Stock > 0);
        }

        
    }
}