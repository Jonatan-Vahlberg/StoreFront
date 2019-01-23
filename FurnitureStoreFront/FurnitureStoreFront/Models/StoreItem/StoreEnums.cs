using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FurnitureStoreFront.Models.StoreItem
{
    public enum Color
    {
        Black = 1,
        White = 2,
        Beige = 4,
        Wood = 8,
        DarkWood = 16,
        Silvermetal = 32,
        VintageRed = 64,
        VintageBlack = 128
    }

    public enum Material
    {
        Wood = 1,
        Metal = 2,
        Leather = 4,
        Plastic = 8,
        Cotton = 16,
        Fabric = 32
    }

    public enum ChairType
    {
        KitchenChair = 1,
        LawnChair,
        DeskChair,
        ArmChair

    }

    public enum TableType
    {
        KitchenTable = 1,
        LawnTable,
        WritingDesk,
        CoffeTable
    }

    public enum BedType
    {
        Queensizedbed = 1,
        Twinsizedbed,
        Studicouch,
    }

    public class EnumMethods
    {

        /// <summary>
        /// Creating a Image link based on Color material and Type of furniture
        /// </summary>
        /// <param name="color">Color of furniture</param>
        /// <param name="material">Material of furniture</param>
        /// <param name="type">Type of furniture</param>
        /// <param name="typeInt">A integer for identifying the type</param>
        /// <returns></returns>
        public static string imageLink(Color color, Material material,string type, int typeInt)
        {
            string ColorString = Enum.GetName(typeof(Color), color);
            string MaterialString = Enum.GetName(typeof(Material), material);
            string TypeString = "";
            if (type.Equals("Chair"))
            {
                ChairType ct = (ChairType)typeInt;
                TypeString = Enum.GetName(typeof(ChairType),ct);
            }
            else if (type.Equals("Table"))
            {
                TableType tt = (TableType)typeInt;
                TypeString = Enum.GetName(typeof(TableType), tt);
            }
            else if (type.Equals("Bed"))
            {
                BedType bt = (BedType)typeInt;
                TypeString = Enum.GetName(typeof(BedType), bt);
            }

            return $"{ColorString}_{MaterialString}_{TypeString}.jpg";
        }

        /// <summary>
        /// Gets and enums Key as string to be used in Product view
        /// </summary>
        /// <param name="type">Type of Enum</param>
        /// <param name="typeInt">A integer for identifying the type</param>
        /// <returns></returns>
        public static string GetEnumValue(string type,int typeInt)
        {
            string TypeString = "";

            switch (type)
            {
                case "Chair":
                    ChairType ct = (ChairType)typeInt;
                    TypeString = Enum.GetName(typeof(ChairType), ct);
                    return TypeString;
                case "Table":
                    TableType tt = (TableType)typeInt;
                    TypeString = Enum.GetName(typeof(TableType), tt);
                    return TypeString;
                case "Bed":
                    BedType bt = (BedType)typeInt;
                    TypeString = Enum.GetName(typeof(BedType), bt);
                    return TypeString;
                case "Color":
                    Color cot = (Color)typeInt;
                    TypeString = Enum.GetName(typeof(Color), cot);
                    return TypeString;
                case "Material":
                    Material mt = (Material)typeInt;
                    TypeString = Enum.GetName(typeof(Material), mt);
                    return TypeString;

            }
            return "Unkown";
        }
    }
}
