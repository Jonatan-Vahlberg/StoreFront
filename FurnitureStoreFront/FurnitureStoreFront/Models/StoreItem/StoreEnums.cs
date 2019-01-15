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
        Darkwood = 16,
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
        WirtingDesk,
        CoffeTable
    }

    public enum BedType
    {
        Queensizedbed = 1,
        Twinsizedbed,
        Studicouch,
    }
}