using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FurnitureStoreFront.Models.StoreItem
{
    /// <summary>
    /// Base Class for furniture items
    /// </summary>
    public class Furniture
    {
        #region Item Properties
        /// <summary>
        /// Name of item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// id of the item
        /// </summary>
        public uint id { get; }

        /// <summary>
        /// price of the item
        /// </summary>
        public uint Price { get; }

        /// <summary>
        /// What room the item belongs to used in tag system
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// Color/colors of the item (enumflags)
        /// </summary>
        public Color Colors { get; }

        /// <summary>
        /// Material or materials of the item in qustion (enumflags)
        /// </summary>
        public Material Material { get; }

        /// <summary>
        /// Type of furniture set in constructor and acsessed in program from get Method
        /// </summary>
        public string TypeOfFurniture { get; set; }

        /// <summary>
        /// Global statistic of item purchase
        /// Used in marketing strategy
        /// </summary>
        public uint TotalPurchases;

        public int TagInt { get; set; }


        public string ImageLink { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the protected variable TypeOfFurniture
        /// </summary>
        /// <returns><see cref="TypeOfFurniture"/></returns>
        public string GetTypeOfFurniture()
        {
            return this.TypeOfFurniture;
        }
        
        /// <summary>
        /// Gets global stat of item
        /// Used in marketing
        /// </summary>
        /// <returns>TotalPurchase</returns>
        public uint GetTotalPurchases()
        {
            return TotalPurchases;
        }

        /// <summary>
        /// Swts the TotalPurchases if b is true
        /// otherwise sets it to 0
        /// </summary>
        /// <param name="b">bool in checking that value exsists</param>
        /// <param name="ui">uint of amount of purchases</param>
        public void SetTotalPurchases(bool b, uint ui)
        {
            if (b)
            {
                this.TotalPurchases = ui;
            }
            this.TotalPurchases = 0;

        }

        #endregion
        #region Constructors
        /// <summary>
        /// Default constructor of a store item
        /// </summary>
        /// <param name="name">name of item</param>
        /// <param name="id">id of item</param>
        /// <param name="price">price of item</param>
        /// <param name="room">room of item</param>
        /// <param name="colors">Color or colors of item</param>
        /// <param name="material">material or materials of item</param>
        public Furniture(string name, uint id, uint price, string room, Color colors, Material material)
        {
            this.Name = name;
            this.id = id;
            this.Price = price;
            this.Room = room;
            this.Colors = colors;
            this.Material = material;
        }
        
        #endregion

    }
    
    #region Sub Classes

    public class Chair : Furniture
    {
        public uint AmountOFChairs { get; }

        public ChairType ChairType { get; set; }

        private void SetChairType(string room)
        {
            if (room.Equals("Bedroom"))
            {
                this.ChairType = ChairType.DeskChair;
                this.TagInt = 11;
                return;
            }
            else if (room.Equals("Livingroom"))
            {
                this.ChairType = ChairType.ArmChair;
                this.TagInt = 21;

                return;
            }
            else if (room.Equals("Outdoors"))
            {
                this.ChairType = ChairType.LawnChair;
                this.TagInt = 41;
                return;
            }
            else if(room.Equals("Kitchen"))
            {
                this.ChairType = ChairType.KitchenChair;
                this.TagInt = 31;

                return;
            }
        }

        public Chair(string name, uint id, uint price, string room, Color colors, Material material, uint amountOfChairs, uint tp): base(name, id ,price, room, colors, material)
        {
            this.AmountOFChairs = amountOfChairs;
            this.TypeOfFurniture = "Chair";
            SetChairType(this.Room);
            this.SetTotalPurchases(true, tp);
            this.ImageLink = StoreItem.EnumMethods.imageLink(this.Colors, this.Material,"Chair", (int)this.ChairType);
        }
    }

    public class Table : Furniture
    {
        public double AreaofTable { get; }

        public TableType TableType;

        protected void SetTableType(string room)
        {
            if (room.Equals("Bedroom"))
            {
                this.TableType = TableType.WritingDesk;
                this.TagInt = 12;

                return;
            }
            else if (room.Equals("Livingroom"))
            {
                this.TableType = TableType.CoffeTable;
                this.TagInt = 22;

                return;
            }
            else if (room.Equals("Outdoors"))
            {
                this.TableType = TableType.LawnTable;
                this.TagInt = 32;

                return;
            }
            else
            {
                this.TableType = TableType.KitchenTable;
                this.TagInt = 42;

                return;
            }
        }

        public TableType GetTableType()
        {
            return this.TableType;
        }

        public Table(string name, uint id, uint price, string room, Color colors, Material material, double areaOfTable, uint tp) : base(name, id, price, room, colors, material)
        {
            this.AreaofTable = areaOfTable;
            this.TypeOfFurniture = "Table";
            SetTableType(this.Room);
            this.SetTotalPurchases(true, tp);
            this.ImageLink = StoreItem.EnumMethods.imageLink(this.Colors, this.Material, "Table", (int)this.TableType);


        }
    }

    public class Bed : Furniture
    {
        public uint BedWidth { get; }
        public uint BedLength { get; }

        public BedType BedType { get; }

        

        public Bed(string name, uint id, uint price, string room, Color colors, Material material, uint bedWidth,uint bedLength,BedType bedType, uint tp) : base(name, id, price, room, colors, material)
        {
            this.BedWidth = bedWidth;
            this.BedLength = bedLength;
            this.BedType = bedType;
            this.TypeOfFurniture = "Bed";
            this.SetTotalPurchases(true, tp);
            if(this.BedType == BedType.Studicouch)
            {
                this.TagInt = 23;

            }
            else
            {
                this.TagInt = 13;
            }
            this.ImageLink = StoreItem.EnumMethods.imageLink(this.Colors, this.Material, "Bed", (int)this.BedType);


        }
    }

    #endregion
}