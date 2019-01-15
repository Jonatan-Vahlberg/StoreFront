using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FurnitureStoreFront.Models;

namespace FurnitureStoreFront.Models.StoreFront
{

    

    public class StoreFront
    {
        public static readonly string StoreListPath = @"C:\Users\jonat\Source\Repos\StoreFront\FurnitureStoreFront\FurnitureStoreFront\App_Data\_REGISTRY\STORELIST.json";
        public static string UserListPath = @"C:\Users\jonat\Source\Repos\StoreFront\FurnitureStoreFront\FurnitureStoreFront\App_Data\_REGISTRY\USERLIST.json";

        /// <summary>
        /// List of all customers dragged from JSON file
        /// </summary>
        public List<User.Customer> CustomerList = User.Customer.getUsers();

        /// <summary>
        /// List of all Store items dragged from JSON file
        /// </summary>
        public List<StoreItem.Furniture> StoreStock = Files.WorkingWithJSON<StoreItem.Furniture>.GetData(StoreListPath);


        private static List<StoreItem.Furniture> FillStockList()
        {
            List<StoreItem.Furniture> StockList = new List<StoreItem.Furniture>();

            StockList.Add(new StoreItem.Chair("Black Kitchen Chair",(uint)(StockList.Count+1),125,"Kitchen",(StoreItem.Color)17,StoreItem.Material.Wood,1));
            StockList.Add(new StoreItem.Chair("Red Leather Armchair",(uint)(StockList.Count+1),870,"Livingroom",(StoreItem.Color)64,StoreItem.Material.Leather,1));
            StockList.Add(new StoreItem.Chair("Plastic Lawn chairs",(uint)(StockList.Count+1),45,"Outdoors",(StoreItem.Color)2,StoreItem.Material.Plastic,4));

            StockList.Add(new StoreItem.Table("Darkwood Writing Desk", (uint)(StockList.Count + 1), 768, "Bedroom", (StoreItem.Color)16, StoreItem.Material.Wood,2.5));
            StockList.Add(new StoreItem.Table("Beige Coffe Table", (uint)(StockList.Count + 1), 456, "Livingroom", (StoreItem.Color)4, StoreItem.Material.Wood, 0.8));
            StockList.Add(new StoreItem.Table("Plastic Lawn Table Two Piece", (uint)(StockList.Count + 1), 199, "Outdoors", (StoreItem.Color)2, StoreItem.Material.Plastic, 6.3));

            StockList.Add(new StoreItem.Bed("White Bedframe with cotton matress", (uint)(StockList.Count + 1), 3399, "Bedroom", (StoreItem.Color)18, (StoreItem.Material)17,90,210,StoreItem.BedType.Twinsizedbed));
            StockList.Add(new StoreItem.Bed("Black Leather Vintage Studiocouch", (uint)(StockList.Count + 1), 8999, "Livingroom", (StoreItem.Color)128, (StoreItem.Material)22,90,190, StoreItem.BedType.Studicouch));
            
            

            return StockList;
        }
    }
}