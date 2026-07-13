using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class APSaveData
{
    public ArchipelagoConnectionConfiguration ConnectionConfiguration;

    public List<ArchipelagoLocation> locationsCache;
    public List<SerializedKeyValuePair> itemsCache;

    public DeckCache deckCache;

    public RelationshipStatus Moment1Relationships;
    public RelationshipStatus Moment2Relationships;
    public RelationshipStatus Moment3Relationships;
    public RelationshipStatus Moment4Relationships;
    public RelationshipStatus Moment5Relationships;
}

[System.Serializable]
public struct SerializedKeyValuePair
{
    public ArchipelagoItem Key;
    public int Value;

    public SerializedKeyValuePair(ArchipelagoItem key, int value)
    {
        this.Key = key;
        this.Value = value;
    }

    public static implicit operator SerializedKeyValuePair(KeyValuePair<ArchipelagoItem, int> kvp)
    {
        SerializedKeyValuePair skvp = new SerializedKeyValuePair();
        skvp.Key = kvp.Key;
        skvp.Value = kvp.Value;
        return skvp;
    }
}

[System.Serializable]
public class RelationshipStatus
{
    // if the player actually played and beat this level
    public bool set;

    public int moment;
    public RelationshipStats MomRelationship;
    public RelationshipStats GrandmaRelationship;
    public RelationshipStats CousinRelationship;
    public RelationshipStats UncleRelationship;

    public RelationshipStatus(int moment)
    {
        this.moment = moment;
        this.set = false;

        MomRelationship = new();
        MomRelationship.currentValue = 0;
        MomRelationship.maxValue = 300;
        MomRelationship.relationshipQuota = 75;

        GrandmaRelationship = new();
        GrandmaRelationship.currentValue = 0;
        GrandmaRelationship.maxValue = 300;
        GrandmaRelationship.relationshipQuota = 75;

        CousinRelationship = new();
        CousinRelationship.currentValue = 0;
        CousinRelationship.maxValue = 300;
        CousinRelationship.relationshipQuota = 75;

        UncleRelationship = new();
        UncleRelationship.currentValue = 0;
        UncleRelationship.maxValue = 300;
        UncleRelationship.relationshipQuota = 75;
    }

    public void Reset()
    {
        set = false;
        MomRelationship.currentValue = 0;
        GrandmaRelationship.currentValue = 0;
        CousinRelationship.currentValue = 0;
        UncleRelationship.currentValue = 0;
    }
}

[System.Serializable]
public class ArchipelagoConnectionConfiguration
{
    public string serverUrl = "multiworld.gg:59641";
    public string GAME_NAME = "Midwest Goodbye";
    public string slotName = "Player1";
    public string password = "";
}

[System.Serializable]
public class DeckCache
{
    public List<SerializedCard> cards;
    public List<ModifierTypes> modifiers;

    public enum StampTypes
    {
        Extrovert,
        Repitition,
        Overthinking,
        Confidence,
        Mumble,
    }

    public enum ModifierTypes
    {
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
        Duplicate,
    }
}

[System.Serializable]
public class SerializedCard
{
    public float Cost;
    public CardEmotion Emotion;
    public CardIntention Intention;
    public List<DeckCache.StampTypes> Stamps;

    public SerializedCard(CardData card)
    {
        this.Cost = card.GetBaseEnergyCost();
        this.Emotion = card.GetEmotion();
        this.Intention = card.GetIntention();
        this.Stamps = new();
        
        foreach(var stamp in card.Stamps)
        {
            if (stamp is ReturnToHandStamp)
                Stamps.Add(DeckCache.StampTypes.Repitition);
            if (stamp is DrawExtraCardStamp)
                Stamps.Add(DeckCache.StampTypes.Overthinking);
            if (stamp is EnergyBonusStamp)
                Stamps.Add(DeckCache.StampTypes.Extrovert);
            if (stamp is MumbleStamp)
                Stamps.Add(DeckCache.StampTypes.Mumble);
            if (stamp is RelationshipAffectorStamp)
                Stamps.Add(DeckCache.StampTypes.Confidence);
        }
    }
}