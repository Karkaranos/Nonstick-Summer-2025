using System.Collections.Generic;
using UnityEngine;

public enum ArchipelagoItem 
{
    None,

    Moment1, Moment2, Moment3, Moment4, Moment5,

    SilentButton,
    DrawButton,
    DiscardButton,

    WittyQuestionCard,
    WittyStatementCard,
    SappyQuestionCard,
    SappyStatementCard,
    AssertiveQuestionCard,
    AssertiveStatementCard,

    WittySticker,
    SappySticker,
    AssertiveSticker,

    StatementSticker,
    QuestionSticker,

    ExtrovertStamp,
    RepititionStamp,
    OverthinkingStamp,
    ConfidenceStamp,
    MumbleStamp,

    Scissors,
    DuplicateCard,

    // Moment 1

    Moment1_Mom,
    Moment1_Grandma,
    
    Moment1_FramedFamilyPhoto,
    Moment1_FridgeMagnets,

    // Moment 2

    Moment2_Mom,
    Moment2_Grandma,
    Moment2_Cousin,
    Moment2_Uncle,

    Moment2_Cake,
    Moment2_Present,

    // Moment 3
    Moment3_Mom,
    Moment3_Grandma,
    Moment3_Cousin,
    Moment3_Uncle,

    Moment3_WaterBottle,
    Moment3_Backpack,

    // Moment 4
    Moment4_Mom,
    Moment4_Grandma,
    Moment4_Cousin,
    Moment4_Uncle,

    Moment4_Phone,

    // Moment 5
    Moment5_Phone,

    // Filler
    Random_Card,
    Random_Modifier,

    // Victory
    Victory_Event,
}

public static class ArchipelagoItemNameMapping
{
    private static Dictionary<string, ArchipelagoItem> nameToItemMap = null; 

    private static Dictionary<string, ArchipelagoItem> CreateNameMapping()
    {
        nameToItemMap = new();

        nameToItemMap.Add("Moment 1", ArchipelagoItem.Moment1);
        nameToItemMap.Add("Moment 2", ArchipelagoItem.Moment2);
        nameToItemMap.Add("Moment 3", ArchipelagoItem.Moment3);
        nameToItemMap.Add("Moment 4", ArchipelagoItem.Moment4);
        nameToItemMap.Add("Moment 5", ArchipelagoItem.Moment5);

        nameToItemMap.Add("Silent Button", ArchipelagoItem.SilentButton);
        nameToItemMap.Add("Draw Button", ArchipelagoItem.DrawButton);
        nameToItemMap.Add("Discard Button", ArchipelagoItem.DiscardButton);

        nameToItemMap.Add("Witty Question Card", ArchipelagoItem.WittyQuestionCard);
        nameToItemMap.Add("Witty Statement Card", ArchipelagoItem.WittyStatementCard);
        nameToItemMap.Add("Sappy Question Card", ArchipelagoItem.SappyQuestionCard);
        nameToItemMap.Add("Sappy Statement Card", ArchipelagoItem.SappyStatementCard);
        nameToItemMap.Add("Assertive Question Card", ArchipelagoItem.AssertiveQuestionCard);
        nameToItemMap.Add("Assertive Statement Card", ArchipelagoItem.AssertiveStatementCard);

        nameToItemMap.Add("Witty Sticker", ArchipelagoItem.WittySticker);
        nameToItemMap.Add("Sappy Sticker", ArchipelagoItem.SappySticker);
        nameToItemMap.Add("Assertive Sticker", ArchipelagoItem.AssertiveSticker);

        nameToItemMap.Add("Statement Sticker", ArchipelagoItem.StatementSticker);
        nameToItemMap.Add("Question Sticker", ArchipelagoItem.QuestionSticker);

        nameToItemMap.Add("Extrovert Stamp", ArchipelagoItem.ExtrovertStamp);
        nameToItemMap.Add("Repitition Stamp", ArchipelagoItem.RepititionStamp);
        nameToItemMap.Add("Overthinking Stamp", ArchipelagoItem.OverthinkingStamp);
        nameToItemMap.Add("Confidence Stamp", ArchipelagoItem.ConfidenceStamp);
        nameToItemMap.Add("Mumble Stamp", ArchipelagoItem.MumbleStamp);

        nameToItemMap.Add("Scissors", ArchipelagoItem.Scissors);
        nameToItemMap.Add("Duplicate Card", ArchipelagoItem.DuplicateCard);

        // Moment 1
        nameToItemMap.Add("Moment 1: Mom", ArchipelagoItem.Moment1_Mom);
        nameToItemMap.Add("Moment 1: Grandma", ArchipelagoItem.Moment1_Grandma);

        // TODO: toy chest
        nameToItemMap.Add("Moment 1: Framed Family Photo", ArchipelagoItem.Moment1_FramedFamilyPhoto);
        nameToItemMap.Add("Moment 1: Fridge Magnets", ArchipelagoItem.Moment1_FridgeMagnets);
        // TODO: the bed?

        // Moment 2
        nameToItemMap.Add("Moment 2: Mom", ArchipelagoItem.Moment2_Mom);
        nameToItemMap.Add("Moment 2: Grandma", ArchipelagoItem.Moment2_Grandma);
        nameToItemMap.Add("Moment 2: Cousin", ArchipelagoItem.Moment2_Cousin);
        nameToItemMap.Add("Moment 2: Uncle", ArchipelagoItem.Moment2_Uncle);

        nameToItemMap.Add("Moment 2: Cake", ArchipelagoItem.Moment2_Cake);
        nameToItemMap.Add("Moment 2: Present", ArchipelagoItem.Moment2_Present);
        //TODO: the bed?

        // Moment 3
        nameToItemMap.Add("Moment 3: Phone", ArchipelagoItem.Moment3_Mom);
        nameToItemMap.Add("Moment 3: Grandma", ArchipelagoItem.Moment3_Grandma);
        nameToItemMap.Add("Moment 3: Cousin", ArchipelagoItem.Moment3_Cousin);
        nameToItemMap.Add("Moment 3: Uncle", ArchipelagoItem.Moment3_Uncle);

        nameToItemMap.Add("Moment 3: Water Bottle", ArchipelagoItem.Moment3_WaterBottle);
        nameToItemMap.Add("Moment 4: Water Bottle", ArchipelagoItem.Moment3_WaterBottle); // sorry guys i made a typo
        nameToItemMap.Add("Moment 3: Backpack", ArchipelagoItem.Moment3_Backpack);

        // Moment 4
        nameToItemMap.Add("Moment 4: Mom", ArchipelagoItem.Moment4_Mom);
        nameToItemMap.Add("Moment 4: Grandma", ArchipelagoItem.Moment4_Grandma);
        nameToItemMap.Add("Moment 4: Cousin", ArchipelagoItem.Moment4_Cousin);
        nameToItemMap.Add("Moment 4: Uncle", ArchipelagoItem.Moment4_Uncle);

        nameToItemMap.Add("Moment 4: Phone", ArchipelagoItem.Moment4_Phone);
        //TODO: the bed?

        // Moment 5:
        nameToItemMap.Add("Moment 5: Phone", ArchipelagoItem.Moment5_Phone);

        nameToItemMap.Add("Random Card", ArchipelagoItem.Random_Card);
        nameToItemMap.Add("Random Modifier", ArchipelagoItem.Random_Modifier);

        nameToItemMap.Add("Victory Event", ArchipelagoItem.Victory_Event);

        return nameToItemMap;
    }

    public static ArchipelagoItem GetItem(string name)
    {
        if (nameToItemMap == null)
            nameToItemMap = CreateNameMapping();

        return nameToItemMap[name];
    }
}