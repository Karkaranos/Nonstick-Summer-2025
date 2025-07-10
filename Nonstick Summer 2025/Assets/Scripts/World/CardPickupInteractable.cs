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

using System.Collections;
using UnityEngine;
using NaughtyAttributes;

public class CardPickupInteractable : MonoBehaviour, IInteractable
{
    [SerializeField, Required]
    private GameObject collectedParticlesPrefab;

    // This game object will have ONE of these two:
    private CardDisplay dialogueCardDisplay;
    private ModifierCardDisplay modifierCardDisplay;

    private RectTransform rectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

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
            DeckManager.AddCardCopy(dialogueCardDisplay.cardData);
            Debug.Log($"Added dialogue card: {dialogueCardDisplay.cardData.name} to deck");
        }
        if (modifierCardDisplay != null)
        {
            ModifierManager.AddCard(modifierCardDisplay.modifierData);
            Debug.Log($"Added modifier card: {modifierCardDisplay.modifierData.name} to deck");
        }

        StartCoroutine(CollectAnimation());
    }

    /// <summary>
    /// I felt like there should be some kind of animation. wasnt sure what it should be tho, so its a hardcoded backflip rn
    /// </summary>
    /// <returns></returns>
    private IEnumerator CollectAnimation()
    {
        //TODO: change animation to anything else

        float animseconds = 2;
        float numberOfBackflips = 3;
        float height = 2;
        Vector3 startPos = rectTransform.position;
        Vector3 startRotation = rectTransform.eulerAngles;

        float timeStarted = Time.time;
        float t=0;
        while(t<1)
        {
            t = (Time.time-timeStarted) / animseconds;

            // if this code doesnt make sense to you then u shouldve paid more attention in ur trig class
            float y = Mathf.Sin(Mathf.PI * t) * height;
            var pos = startPos + new Vector3(0, y, 0);

            var rot = startRotation + new Vector3(t * numberOfBackflips * 360, 0, 0);

            rectTransform.position = pos;
            rectTransform.eulerAngles = rot;

            yield return null;
        }
        AfterCollectAnimationFinished();
    }

    private void AfterCollectAnimationFinished()
    {

        Destroy(this.gameObject);
    }
}
