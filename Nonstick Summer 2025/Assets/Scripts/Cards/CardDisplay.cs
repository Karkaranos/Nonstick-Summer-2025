using System;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] [Tooltip("Set this for debug only")]
    private CardData card;

    public void RefreshDisplay()
    {
        if(card == null)
        {
            Debug.LogWarning("No card is set.");
            return;
        }

        throw new NotImplementedException("UpdateDisplay not implemented yet.");
    }

    public void SetCard(CardData newCard)
    { 
        card = newCard; 
        RefreshDisplay();
    }
}
