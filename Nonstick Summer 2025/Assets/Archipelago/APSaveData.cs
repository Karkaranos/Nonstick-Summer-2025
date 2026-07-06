using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class APSaveData
{
    public ArchipelagoConnectionConfiguration ConnectionConfiguration;

    public List<ArchipelagoLocation> locationsCache;
    public List<KeyValuePair<ArchipelagoItem,int>> itemsCache;

    public RelationshipStatus Moment1Relationships;
    public RelationshipStatus Moment2Relationships;
    public RelationshipStatus Moment3Relationships;
    public RelationshipStatus Moment4Relationships;
    public RelationshipStatus Moment5Relationships;
}

[System.Serializable]
public class RelationshipStatus
{
    // if the player actually played and beat this level
    public bool set;

    public int moment;
    public int MomRelationship;
    public int GrandmaRelationship;
    public int CousinRelationship;
    public int UncleRelationship;
}

[System.Serializable]
public class ArchipelagoConnectionConfiguration
{
    public string serverUrl = "multiworld.gg:59641";
    public string GAME_NAME = "Midwest Goodbye";
    public string slotName = "Player1";
    public string password = "";
}