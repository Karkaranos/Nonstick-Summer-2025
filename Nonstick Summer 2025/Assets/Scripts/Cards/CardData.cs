/*****************************************************************************
* File Name :         CardData.cs
* Author :            Toby
* Creation Date :     June 6, 2025
*
* Brief Description : Data container for cards.
* Card Data is a partial class, see CardStampCollection for _modifier logic
*
* TODO:
* modifier implementation (modify getter functions)
* 
*****************************************************************************/

using NaughtyAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public partial class CardData : ScriptableObject
{
    [HideInInspector] public Action OnCardValueChanged;

    [OnValueChanged("Debug_InvokeOnCardValueChanged")]
    [Tooltip("ADDS this value to the players energy on played. (Leave negative to subtract energy)")] // i am such a freak
    [SerializeField][Label("Energy Cost")] private float _energyCost = -2;

    [OnValueChanged("Debug_InvokeOnCardValueChanged")]
    [SerializeField][Label("Emotion")] private CardEmotion _emotion;
    [OnValueChanged("Debug_InvokeOnCardValueChanged")]
    [SerializeField] [Label("Intention")] private CardIntention _intention;

    // modifiers later?

    #region static utilities

    public static CardData CopyCard(CardData card)
    {
        CardData copy = ScriptableObject.CreateInstance<CardData>();
        copy._emotion = card._emotion;
        copy._intention = card._intention;
        copy._energyCost = card._energyCost;

        return copy;
    }

    #endregion

    #region Getters and Setters

    [Header("Debug")]
    [ShowNativeProperty]
    public float EnergyCost {
        get { return GetEnergyCost(); }
        set { SetEnergyCost(value); }
    }
    [ShowNativeProperty]
    public CardIntention Intention {
        get { return GetIntention(); }
        set { SetIntention(value); }
    }
    [ShowNativeProperty]
    public CardEmotion Emotion{
        get { return GetEmotion(); }
        set { _emotion = value; OnCardValueChanged.Invoke(); }
    }

    public float GetEnergyCost() 
    {
        float newCost = _energyCost;

        if(_stamps.Count > 0)
        {
            foreach (ModifierStamp stamp in _stamps)
            {
                if (stamp.type == typeof(CardStatAffectorStamp))
                    ((CardStatAffectorStamp)stamp).ModifyEnergyCost(ref newCost);
            }
        }
        return newCost; 
    }
    public CardIntention GetIntention() { return _intention; }
    public CardEmotion GetEmotion() { return _emotion; }


    public float GetRelationshopChange(DialogueOption dialogueOption)
    {
        float newRelationshipChange = dialogueOption.ChangeInRelationshipStatus;

        foreach (ModifierStamp stamp in _stamps)
        {
            if (stamp.type == typeof(CardStatAffectorStamp))
                ((CardStatAffectorStamp)stamp).ModifyEnergyCost(ref newRelationshipChange);
        }
        return newRelationshipChange;
    }

    public void SetEnergyCost(float energyCost)
    {
        if (energyCost == _energyCost) return;
        _energyCost = energyCost;
        OnCardValueChanged.Invoke();
    }

    public void SetIntention(CardIntention intention)
    {
        if (intention == _intention) return;
        _intention = intention;
        OnCardValueChanged.Invoke();
    }

    public void SetEmotion(CardEmotion emotion)
    {
        if (emotion == _emotion) return;
        _emotion = emotion;
        OnCardValueChanged.Invoke();
    }


    #endregion


    public CardData CopyCard()
    {
        CardData copy = ScriptableObject.CreateInstance<CardData>();
        copy._emotion = _emotion;
        copy._intention = _intention;
        copy._energyCost = _energyCost;

        copy._stamps = new List<ModifierStamp>(_stamps);

        return copy;
    }

    #region debug
    private void Debug_InvokeOnCardValueChanged()
    {
        OnCardValueChanged.Invoke();
    }

    public static CardData NewCard (int EnergyCost, CardEmotion Emotion, CardIntention Intention)
    {
        CardData newcard = ScriptableObject.CreateInstance<CardData>();
        //newcard._energyCost = EnergyCost; 
        newcard._emotion = Emotion;

        //this makes the EnergyCost int null and void but i don't wanna fuck with this function too much for the time being
        newcard._energyCost = MoodManager.emotions[Emotion].defaultEnergyCost;
        newcard._intention = Intention;
        return newcard;
    }

    #endregion
}

public enum CardIntention
{ 
    NotSelected, // Error case
    Expression,
    Observation,
    Question, // Will update these later when Intentions are finalized
}

public enum CardEmotion
{
    NotSelected, // Error case
    Charming,
    Assertive,
    Sappy, // Will update these later when Emotions are finalized
}
