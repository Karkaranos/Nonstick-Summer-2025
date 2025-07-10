/*****************************************************************************
* File Name :         CardPickupInteractable.cs
* Author :            Toby
* Creation Date :     July 10, 2025
*
* Brief Description : Interactable gameobject that the player can pickup in the
* world.
* 
* toby comments: god i wish we did some kind of inheritence system with CardData and
* ModifierCards but whatever
* 
*****************************************************************************/

using UnityEngine;

public class CardPickupInteractable : MonoBehaviour, IInteractable
{
    // This game object will have ONE of these two:
    private CardDisplay dialogueCardDisplay;
    private ModifierCardDisplay modifierCardDisplay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueCardDisplay = GetComponent<CardDisplay>();
        modifierCardDisplay = GetComponent<ModifierCardDisplay>();

        // if theyre both null OR both defined
        if( (dialogueCardDisplay != null) == (modifierCardDisplay != null))
        {
            Debug.LogError("Card pickup can not have a modifier and a dialogue");
        }
    }

    /// <summary>
    /// Triggered when player interacts with this gameobject
    /// </summary>
    public void Interact(GameObject player)
    {
        if(dialogueCardDisplay != null)
        {
            DeckManager.AddCard(dialogueCardDisplay.cardData);
        }
        if (modifierCardDisplay != null)
        {
            ModifierManager.AddCard(modifierCardDisplay.modifierData);
        }
    }
}
