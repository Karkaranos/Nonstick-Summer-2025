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

    // Moment 3
    Moment3_Mom,
    Moment3_Grandma_Route_1,
    Moment3_Grandma_Route_2,
    Moment3_Uncle,
    Moment3_Cousin,

    Moment3_Interact_Water_Bottle,
    Moment3_Interact_Backpack,

    CardPickup_KitchenTrash,
    CardPickup_UnderLamp,

    Moment3_Complete,

    // Moment 4
    Moment4_Mom_Route_1,
    Moment4_Mom_Route_2,
    Moment4_Grandma,
    Moment4_Cousin_Route_1,
    Moment4_Cousin_Route_2,
    Moment4_Uncle,

    Moment4_Interact_Phone,

    CardPickup_Laundry,
    CardPickup_DiningRoomCabinet,

    Moment4_Complete,

    // Moment 4
    Moment5_Mom_Route_1,
    Moment5_Mom_Route_2,
    Moment5_Mom_Route_3,
    Moment5_Grandma_Route_1,
    Moment5_Grandma_Route_2,
    Moment5_Grandma_Route_3,
    Moment5_Cousin_Route_1,
    Moment5_Cousin_Route_2,
    Moment5_Cousin_Route_3,
    Moment5_Uncle_Route_1,
    Moment5_Uncle_Route_2,
    Moment5_Uncle_Route_3,

    Moment5_Interact_Phone,

    CardPickup_TopBookcase,
    CardPickup_InSink,

    Moment5_Complete,

    Victory_Location,
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


        // Moment 3
        locationToNameMap.Add(ArchipelagoLocation.Moment3_Grandma_Route_1, "Moment 3: Talk to Grandma - Route 1");
        locationToNameMap.Add(ArchipelagoLocation.Moment3_Grandma_Route_2, "Moment 3: Talk to Grandma - Route 2");
        locationToNameMap.Add(ArchipelagoLocation.Moment3_Cousin, "Moment 3: Talk to Cousin");
        locationToNameMap.Add(ArchipelagoLocation.Moment3_Mom, "Moment 3: Call Mom");
        locationToNameMap.Add(ArchipelagoLocation.Moment3_Uncle, "Moment 3: Talk to Uncle");

        locationToNameMap.Add(ArchipelagoLocation.Moment3_Interact_Water_Bottle, "Moment 3: Water Bottle Interaction");
        locationToNameMap.Add(ArchipelagoLocation.Moment3_Interact_Backpack, "Moment 3: Backpack Interaction");

        locationToNameMap.Add(ArchipelagoLocation.CardPickup_KitchenTrash, "Card Pickup: Kitchen Trash");
        locationToNameMap.Add(ArchipelagoLocation.CardPickup_UnderLamp, "Card Pickup: Under Lamp");

        locationToNameMap.Add(ArchipelagoLocation.Moment3_Complete, "Complete Moment 3");

        // Moment 4
        locationToNameMap.Add(ArchipelagoLocation.Moment4_Mom_Route_1, "Moment 4: Talk to Mom - Route 1");
        locationToNameMap.Add(ArchipelagoLocation.Moment4_Mom_Route_2, "Moment 4: Talk to Mom - Route 2");
        locationToNameMap.Add(ArchipelagoLocation.Moment4_Cousin_Route_1, "Moment 4: Talk to Cousin - Route 1");
        locationToNameMap.Add(ArchipelagoLocation.Moment4_Cousin_Route_2, "Moment 4: Talk to Cousin - Route 2");
        locationToNameMap.Add(ArchipelagoLocation.Moment4_Grandma, "Moment 4: Talk to Grandma");
        locationToNameMap.Add(ArchipelagoLocation.Moment4_Uncle, "Moment 4: Talk to Uncle");

        locationToNameMap.Add(ArchipelagoLocation.CardPickup_Laundry, "Card Pickup: Laundry");
        locationToNameMap.Add(ArchipelagoLocation.CardPickup_DiningRoomCabinet, "Card Pickup: Dining Room Cabinet");

        locationToNameMap.Add(ArchipelagoLocation.Moment4_Complete, "Complete Moment 4");

        // Moment 5
        locationToNameMap.Add(ArchipelagoLocation.Moment5_Mom_Route_1, "Moment 5: Talk to Mom - Route 1");
        locationToNameMap.Add(ArchipelagoLocation.Moment5_Mom_Route_2, "Moment 5: Talk to Mom - Route 2");
        locationToNameMap.Add(ArchipelagoLocation.Moment5_Mom_Route_3, "Moment 5: Talk to Mom - Route 3");
        locationToNameMap.Add(ArchipelagoLocation.Moment5_Grandma_Route_1, "Moment 5: Talk to Grandma - Route 1");
        locationToNameMap.Add(ArchipelagoLocation.Moment5_Grandma_Route_2, "Moment 5: Talk to Grandma - Route 2");
        locationToNameMap.Add(ArchipelagoLocation.Moment5_Grandma_Route_3, "Moment 5: Talk to Grandma - Route 3");
        locationToNameMap.Add(ArchipelagoLocation.Moment5_Cousin_Route_1, "Moment 5: Talk to Cousin - Route 1");
        locationToNameMap.Add(ArchipelagoLocation.Moment5_Cousin_Route_2, "Moment 5: Talk to Cousin - Route 2");
        locationToNameMap.Add(ArchipelagoLocation.Moment5_Cousin_Route_3, "Moment 5: Talk to Cousin - Route 3");
        locationToNameMap.Add(ArchipelagoLocation.Moment5_Uncle_Route_1, "Moment 5: Talk to Uncle - Route 1");
        locationToNameMap.Add(ArchipelagoLocation.Moment5_Uncle_Route_2, "Moment 5: Talk to Uncle - Route 2");
        locationToNameMap.Add(ArchipelagoLocation.Moment5_Uncle_Route_3, "Moment 5: Talk to Uncle - Route 3");

        locationToNameMap.Add(ArchipelagoLocation.CardPickup_TopBookcase, "Card Pickup: Top of Bookcase");
        locationToNameMap.Add(ArchipelagoLocation.CardPickup_InSink, "Card Pickup: Sink");

        locationToNameMap.Add(ArchipelagoLocation.Moment5_Complete, "Complete Moment 5");

        // end condition
        locationToNameMap.Add(ArchipelagoLocation.Victory_Location, "Victory Location");

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

        if (locationToNameMap.ContainsKey(location) == false)
        {
            Debug.LogError($"No key for {location.ToString()}");
            return null;
        }

        return locationToNameMap[location];
    }
}