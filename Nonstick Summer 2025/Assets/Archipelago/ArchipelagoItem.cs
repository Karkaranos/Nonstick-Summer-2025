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

    Moment1_Mom,
    Moment1_Grandma,
    
    Moment1_FramedFamilyPhoto,
    Moment1_FridgeMagnets
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

        return nameToItemMap;
    }

    public static ArchipelagoItem GetItem(string name)
    {
        if (nameToItemMap == null)
            nameToItemMap = CreateNameMapping();

        return nameToItemMap[name];
    }
}