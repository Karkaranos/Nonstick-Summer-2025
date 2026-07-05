using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class APSaveData
{
    public List<ArchipelagoLocation> locationsCache;
    public List<KeyValuePair<ArchipelagoItem,int>> itemsCache;

    RelationshipStatus Moment1Relationships;
    RelationshipStatus Moment2Relationships;
    RelationshipStatus Moment3Relationships;
    RelationshipStatus Moment4Relationships;
    RelationshipStatus Moment5Relationships;
}

[System.Serializable]
public class RelationshipStatus
{
    public Moment moment;
    public int MomRelationship;
    public int GrandmaRelationship;
    public int CousinRelationship;
    public int UncleRelationship;
}


// move to different script?
public enum Moment
{
    Tutorial,
    Moment1,
    Moment2,
    Moment3,
    Moment4,
    Moment5
}
