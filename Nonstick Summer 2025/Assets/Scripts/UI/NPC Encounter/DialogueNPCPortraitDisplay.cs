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

    [SerializeField] float bobSpeed = 30;

    [Header("Animated Reaction Anchors")]
    [SerializeField] Vector2 momAnchor;
    [SerializeField] Vector2 grandmaAnchor;
    [SerializeField] Vector2 cousinAnchor;
    [SerializeField] Vector2 uncleAnchor;

    private Coroutine portraitAnimation;
    private Vector3 defaultSpritePosition;
    private bool setAny = false;

    private void Start()
    {
        defaultSpritePosition = NPCImage.transform.position;
    }

    public void SetPortraitSprite(DialogueNPC dialogue, Character character)
    {
        ReactionManager.instance.SetCharacter(character);

        if (dialogue == null || dialogue.Portrait == null)
            return;

        if(!setAny)
        {
            NPCImage.sprite = dialogue.Portrait;
        }

        //instantiates animation
        //making a new function to prevent clutter
        if (dialogue.AnimatedReaction != null)
        {
            if (portraitAnimation != null)
                StopCoroutine(portraitAnimation);

            PlayReaction(dialogue, character);
        }
        else if (portraitAnimation == null)
        {
            portraitAnimation = StartCoroutine(UpdateSpriteCoroutine(dialogue.Portrait, character));
        }
    }

    private IEnumerator UpdateSpriteCoroutine(Sprite portrait, Character character, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        var targetPos = defaultSpritePosition + (Vector3.up * 100);
        while (NPCImage.transform.position != targetPos)
        {
            NPCImage.transform.position = Vector3.MoveTowards(NPCImage.transform.position, targetPos, Time.deltaTime * bobSpeed);
            yield return null;
        }
        NPCImage.sprite = portrait;
        while (NPCImage.transform.position != defaultSpritePosition)
        {
            NPCImage.transform.position = Vector3.MoveTowards(NPCImage.transform.position, defaultSpritePosition, Time.deltaTime * bobSpeed);
            yield return null;
        }

        float timeStarted = Time.time;
        while(true)
        {
            NPCImage.transform.position = defaultSpritePosition + new Vector3(0, ( -Mathf.Sin(Time.time - timeStarted) * bobSpeed));
            yield return null;
        }
    }

    // i ended up getting a little confused. sorry for refactoring this, i didnt need to do that
    private Vector2 GetAnchor(Character character)
    {
        switch (character)
        {
            case (Character.Mom):
                return momAnchor;
            case (Character.Grandma):
                return grandmaAnchor;
            case (Character.Cousin):
                return cousinAnchor;
            case(Character.Uncle):
                return uncleAnchor;
            default:
                return Vector2.zero;
        }
    }

    void PlayReaction(DialogueNPC dialogue, Character character)
    {
        GameObject reaction = Instantiate(dialogue.AnimatedReaction);
        reaction.transform.SetParent(this.transform.GetComponentInParent<Transform>());

        DialogueUIController.Instance.activeReaction = reaction;

        reaction.transform.position = GetAnchor(character);

        StartCoroutine(UpdateSpriteCoroutine(dialogue.Portrait, character, 0.5f));
    }
}
