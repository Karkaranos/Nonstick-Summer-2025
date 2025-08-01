/*****************************************************************************
* File Name :         DialogueNPCPortraitDisplay.cs
* Author :            Toby
* Creation Date :     June 27, 2025
*
* Brief Description :  Modular component for NPC combat. Recieves commands from 
*   DialogueUIController.
*   If NPC sprite is null, it uses the sprite from the last dialogue blurb
*   
* TODO: Animations
* 
*****************************************************************************/

using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueNPCPortraitDisplay : MonoBehaviour
{
    [SerializeField, Required] private Image NPCImage;

    [Header("Animated Reaction Anchors")]
    [SerializeField] Vector2 momAnchor;
    [SerializeField] Vector2 grandmaAnchor;
    [SerializeField] Vector2 cousinAnchor;
    [SerializeField] Vector2 uncleAnchor;

    private Coroutine portraitAnimation;

    public void SetPortraitSprite(DialogueNPC dialogue, characters character)
    {
        if (dialogue == null || dialogue.Portrait == null)
            return;

        if(portraitAnimation != null)
            StopCoroutine(portraitAnimation);

        //instantiates animation
        //making a new function to prevent clutter
        if(dialogue.AnimatedReaction != null)
        {

            PlayReaction(dialogue, character);

        }

        portraitAnimation = StartCoroutine(UpdateSpriteCoroutine(dialogue.Portrait));
    }

    // TODO: animation
    private IEnumerator UpdateSpriteCoroutine(Sprite portrait)
    {
        NPCImage.sprite = portrait;
        yield return null;
        portraitAnimation = null;
    }

    void PlayReaction(DialogueNPC dialogue, characters character)
    {

        GameObject reaction = Instantiate(dialogue.AnimatedReaction);
        reaction.transform.SetParent(this.transform.GetComponentInParent<Transform>());

        DialogueUIController.Instance.activeReaction = reaction;

        if(character == characters.Mom)
        {

            reaction.transform.position = momAnchor;
            
        }
        if (character == characters.Grandma)
        {

            reaction.transform.position = grandmaAnchor;

        }
        if (character == characters.Cousin)
        {

            reaction.transform.position = cousinAnchor;

        }
        if (character == characters.Uncle)
        {

            reaction.transform.position = uncleAnchor;

        }

    }
}
