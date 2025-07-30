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
    [SerializeField, Required] private Image NPCReation;

    private Coroutine portraitAnimation;

    public void SetPortraitSprite(DialogueNPC dialogue)
    {
        if (dialogue == null || dialogue.Portrait == null)
            return;

        if(portraitAnimation != null)
            StopCoroutine(portraitAnimation);

        portraitAnimation = StartCoroutine(UpdateSpriteCoroutine(dialogue.Portrait));
    }

    // TODO: animation
    private IEnumerator UpdateSpriteCoroutine(Sprite portrait)
    {
        NPCImage.sprite = portrait;
        yield return null;
        portraitAnimation = null;
    }
}
