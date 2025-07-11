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

    const float backflipSeconds = 1.25f;
    const float backflipsSpeed = 3;
    const float height = 1.5f;

    const float goToPlayerSeconds = 1;

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
        RemoveCollider();
        StaticUtilities.PlayAndDestroyParticle(collectedParticlesPrefab, rectTransform.WorldPosition());

        if (dialogueCardDisplay != null)
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
    /// I felt like there should be some kind of animation. wasnt sure what it should be tho, so its a hardcoded backflip rn.
    /// this code sucx pls dont put it 
    /// </summary>
    /// <returns></returns>
    private IEnumerator CollectAnimation()
    {
        //TODO: change animation to anything else

        Vector3 startPos = rectTransform.position;
        Vector3 startEulers = rectTransform.eulerAngles;
        Vector3 startScale = rectTransform.localScale;

        // Backflips
        float timeStarted = Time.time;
        float t=0;
        while(t<1)
        {
            t = (Time.time-timeStarted) / backflipSeconds;

            // if this code doesnt make sense to you then u shouldve paid more attention in ur trig class
            float y = Mathf.Sin(Mathf.PI * t / 2) * height;
            var pos = startPos + new Vector3(0, y, 0);

            var rot = startEulers + new Vector3(t * backflipsSpeed * 360, 0, 0);

            rectTransform.position = pos;
            rectTransform.eulerAngles = rot;

            yield return null;
        }

        Debug.Log("Confetti particles go here???"); //TODO:

        // Go to her...
        timeStarted = Time.time;
        t = 0;
        startPos = rectTransform.position;
        var startRotation = rectTransform.rotation; 
        while (t < 1)
        {
            Debug.Log(t);
            t = (Time.time - timeStarted) / goToPlayerSeconds;

            // if this code doesnt make sense to you then u shouldve paid more attention in ur trig class

            var pos = Vector3.Lerp(startPos, GameManager.playerTransformRef.position, t * t); // t * t so it gets faster (plug x^2 into desmos and look at 0-1 to see the effect for yourself! it will be mind boggling!!!!!)
            var targetRot = Quaternion.LookRotation(GameManager.playerTransformRef.position - rectTransform.WorldPosition());
            var rot = Quaternion.Lerp(startRotation, targetRot, t * 3);
            var scale = Vector3.Lerp(startScale, Vector3.zero, t);

            rectTransform.position = pos;
            rectTransform.rotation = rot;
            rectTransform.localScale = scale;

            yield return null;
        }

        AfterCollectAnimationFinished();
    }

    private void AfterCollectAnimationFinished()
    {
        StaticUtilities.PlayAndDestroyParticle(collectedParticlesPrefab, rectTransform.WorldPosition());
        Destroy(this.gameObject);
    }

    private void RemoveCollider()
    {
        Destroy(gameObject.GetComponentInChildren<Collider>());
    }
}
