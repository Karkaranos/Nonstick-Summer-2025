using NaughtyAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    [Min(0)]
    [OnValueChanged("Debug_InvokeOnCardValueChanged")]
    [SerializeField][Label("Energy Cost")] private int _energyCost;

    [OnValueChanged("Debug_InvokeOnCardValueChanged")]
    [SerializeField][Label("Emotion")] private CardEmotion _emotion;
    [OnValueChanged("Debug_InvokeOnCardValueChanged")]
    [SerializeField] [Label("Intention")] private CardIntention _intention;

    // modifiers later?

    #region Getters and Setters
    public int EnergyCost{
        get { return _energyCost; }
        set { _energyCost = value; OnCardValueChanged.Invoke(); } }

    public CardIntention Intention { 
        get { return _intention; } 
        set { _intention = value; OnCardValueChanged.Invoke(); } }

    public CardEmotion Emotion{
        get { return _emotion; }
        set { _emotion = value; OnCardValueChanged.Invoke(); }}
    #endregion

    [HideInInspector]
    public Action OnCardValueChanged;

    public static CardData CopyCard(CardData card)
    {
        CardData copy = new CardData();
        copy._emotion = card._emotion;
        copy._intention = card._intention;
        copy._energyCost = card._energyCost;

        return copy;
    }

    #region debug
    private void Debug_InvokeOnCardValueChanged()
    {
        OnCardValueChanged.Invoke();
    }

    #endregion
}

public enum CardIntention
{ 
    NotSelected, // Error case
    Intention1,
    Intention2,
    Intention3, // Will update these later when Intentions are finalized
}

public enum CardEmotion
{
    NotSelected, // Error case
    Yellow,
    Red,
    Blue, // Will update these later when Emotions are finalized
}