using System.Collections.Generic;
using UnityEngine;

public enum ArchipelagoLocation 
{
    None,

    // Moment 1
    Moment1_Mom_Route_1,
    Moment1_Mom_Route_2,
    Moment1_Grandma,

    Moment1_Interact_ToyBox,
    Moment1_Interact_FramedFamilyPhoto,
    Moment1_Interact_FridgeMagnets,

    CardPickup_Chair,
    CardPickup_Cabinet,

    Moment1_Complete,
    
    // Moment 2
    Moment2_Cousin_Route_1,
    Moment2_Cousin_Route_2,
    Moment2_Mom,
    Moment2_Uncle,
    Moment2_Grandma,

    Moment2_Interact_Cake,
    Moment2_Interact_Present,
    Moment2_Interact_Cards,

    CardPickup_BedroomDesk,
    CardPickup_KitchenCounter,
    CardPickup_UnderCouch,

    Moment2_Complete,
}

public static class ArchipelagoLocationNameMapping
{
    private static Dictionary<ArchipelagoLocation, string> locationToNameMap = null; 

    private static Dictionary<ArchipelagoLocation, string> CreateNameMapping()
    {
        locationToNameMap = new();

        locationToNameMap.Add(ArchipelagoLocation.Moment1_Mom_Route_1, "Moment 1: Talk to Mom - Route 1");
        locationToNameMap.Add(ArchipelagoLocation.Moment1_Mom_Route_2, "Moment 1: Talk to Mom - Route 2");
        locationToNameMap.Add(ArchipelagoLocation.Moment1_Grandma, "Moment 1: Talk to Grandma");

        locationToNameMap.Add(ArchipelagoLocation.Moment1_Interact_ToyBox, "Moment 1: Toy Box Interaction");
        locationToNameMap.Add(ArchipelagoLocation.Moment1_Interact_FramedFamilyPhoto, "Moment 1: Framed Family Photo Interaction");
        locationToNameMap.Add(ArchipelagoLocation.Moment1_Interact_FridgeMagnets, "Moment 1: Fridge Magnets Interaction");

        locationToNameMap.Add(ArchipelagoLocation.CardPickup_Chair, "Card Pickup: Chair");
        locationToNameMap.Add(ArchipelagoLocation.CardPickup_Cabinet, "Card Pickup: Cabinet");

        locationToNameMap.Add(ArchipelagoLocation.Moment1_Complete, "Complete Moment 1");

        // Moment 2
        locationToNameMap.Add(ArchipelagoLocation.Moment2_Cousin_Route_1, "Moment 2: Talk to Cousin - Route 1");
        locationToNameMap.Add(ArchipelagoLocation.Moment2_Cousin_Route_2, "Moment 2: Talk to Cousin - Route 2");
        locationToNameMap.Add(ArchipelagoLocation.Moment2_Mom, "Moment 2: Talk to Mom");
        locationToNameMap.Add(ArchipelagoLocation.Moment2_Grandma, "Moment 2: Talk to Grandma");
        locationToNameMap.Add(ArchipelagoLocation.Moment2_Uncle, "Moment 2: Talk to Uncle");

        locationToNameMap.Add(ArchipelagoLocation.Moment2_Interact_Cake, "Moment 2: Cake Interaction");
        locationToNameMap.Add(ArchipelagoLocation.Moment2_Interact_Present, "Moment 2: Present Interaction");
        locationToNameMap.Add(ArchipelagoLocation.Moment2_Interact_Cards, "Moment 2: Cards Interaction");

        locationToNameMap.Add(ArchipelagoLocation.CardPickup_BedroomDesk, "Card Pickup: Bedroom Desk");
        locationToNameMap.Add(ArchipelagoLocation.CardPickup_KitchenCounter, "Card Pickup: Kitchen Counter");
        locationToNameMap.Add(ArchipelagoLocation.CardPickup_UnderCouch, "Card Pickup: Under Couch");

        locationToNameMap.Add(ArchipelagoLocation.Moment2_Complete, "Complete Moment 2");

        /*
# Moment 3
        "Moment 3: Talk to Grandma - Route 1" : 301,
    "Moment 3: Talk to Grandma - Route 2" : 302,
    "Moment 3: Talk to Cousin"            : 303,
    "Moment 3: Call Mom"                  : 304,
    "Moment 3: Talk to Uncle"             : 305,

    "Moment 3: Water Bottle Interaction"              : 311,
    "Moment 3: Backpack Interaction"                  : 312,

    "Card Pickup: Kitchen Trash"          : 321,
    "Card Pickup: Under Lamp"             : 322,

    "Complete Moment 3"                   : 399,

# Moment 4
    "Moment 4: Talk to Mom - Route 1"     : 401,
    "Moment 4: Talk to Mom - Route 2"     : 402,
    "Moment 4: Talk to Cousin - Route 1"  : 403,
    "Moment 4: Talk to Cousin - Route 2"  : 404,
    "Moment 4: Talk to Grandma"           : 405,
    "Moment 4: Talk to Uncle"             : 406,

    "Card Pickup: Laundry"                : 421,
    "Card Pickup: Dining Room Cabinet"    : 422,

    "Complete Moment 4"                   : 499,);
        */

        return locationToNameMap;
    }

    public static ArchipelagoLocation GetLocation(string name)
    {
        if (locationToNameMap == null)
            locationToNameMap = CreateNameMapping();

        return locationToNameMap.GetFirstKeyByValue(name);
    }

    public static string GetLocationName(ArchipelagoLocation location)
    {
        if(location == ArchipelagoLocation.None)
            return null;

        if (locationToNameMap == null)
            locationToNameMap = CreateNameMapping();

        return locationToNameMap[location];
    }
}