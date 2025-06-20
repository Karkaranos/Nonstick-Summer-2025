using UnityEngine;

public enum StampTriggerConditions
{
    None, // basic stat changes
    BeforeCardPlayed, // DialogueManager
    AfterCardPlayed, // DialogueManager
    OnCardDiscarded, //TODO implement
    //TODO think of other trigger conditions?
}
