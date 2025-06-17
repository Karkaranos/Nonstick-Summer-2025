using UnityEngine;

public enum StampTriggerConditions
{
    NOT_SELECTED, // this maaay be renamed if theres any passive effects in the future (ie. seeing how npc will react to card before it is played)
    BeforeCardPlayed, // DialogueManager
    AfterCardPlayed, // DialogueManager
    OnCardDiscarded, //TODO implement
    //TODO think of other trigger conditions?
}
