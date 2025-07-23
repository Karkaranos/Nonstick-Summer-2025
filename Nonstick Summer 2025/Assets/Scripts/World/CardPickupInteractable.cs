/*****************************************************************************
* File Name :         CardPickupInteractable.cs
* Author :            Toby
* Creation Date :     July 10, 2025
*
* Brief Description : Interactable gameobject that the player can pickup in the
* world.
* 
* TODO:
* confirmation UI
* 
* toby comments: god i wish we did some kind of inheritence system with CardData and
* ModifierCards but whatever
* 
*****************************************************************************/

using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using System;
using UnityEditor;

public class CardPickupInteractable : MonoBehaviour, IInteractable
{
    [SerializeField, Required]
    private GameObject collectedParticlesPrefab;

    // This game object will have ONE of these two:
    private CardDisplay dialogueCardDisplay;
    private ModifierCardDisplay modifierCardDisplay;

    private RectTransform rectTransform;
    private Vector3 startPosition;

    [ReadOnly] public int Hash = -1;

    // my magicians hat full of magic numbers
    const float backflipSeconds = 1.25f;
    const float backflipsSpeed = 3;
    const float height = 1.5f;

    const float goToPlayerSeconds = 1;
    private bool interacted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.position;

        dialogueCardDisplay = GetComponent<CardDisplay>();
        modifierCardDisplay = GetComponent<ModifierCardDisplay>();

        // if theyre both null OR both defined
        if( (dialogueCardDisplay != null) == (modifierCardDisplay != null))
        {
            Debug.LogError("Card pickup can not have a modifier and a dialogue");
        }

        Hash = GetCardHashCode();
        CardPickupManager.Instance.InitializePickup(this);
    }

    /// <summary>
    /// Triggered when player interacts with this gameobject
    /// </summary>
    public void Interact(GameObject player)
    {
        if (!interacted)
        {
            interacted = true;
            // TODO: add some kind of popup / confirmation

            RemoveCollider();
            StaticUtilities.PlayAndDestroyParticle(collectedParticlesPrefab, rectTransform.WorldPosition());
            CardPickupManager.Instance.UpdatePickupCollected(this);

            if (dialogueCardDisplay != null)
            {
                print("Call 1 from " + gameObject.name);
                DeckManager.AddCardCopy(dialogueCardDisplay.cardData);
                Debug.Log($"Added dialogue card: {dialogueCardDisplay.cardData.name} to deck");
            }
            if (modifierCardDisplay != null)
            {
                print("Call 2 from " + gameObject.name);
                ModifierManager.AddCard(modifierCardDisplay.modifierData);
                Debug.Log($"Added modifier card: {modifierCardDisplay.modifierData.name} to deck");
            }

            StartCoroutine(CollectAnimation());
        }
    }

    private void RemoveCollider()
    {
        Destroy(gameObject.GetComponentInChildren<Collider>());
    }

    #region Animation 

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

        Debug.Log("Confettii explosion goes here???"); //TODO:

        // Go to her...
        timeStarted = Time.time;
        t = 0;

        if (GameManager.playerTransformRef == null)
        {
            GameManager.playerTransformRef = FindFirstObjectByType<PlayerCamera>().transform;   
        }

        startPos = rectTransform.position;
        var startRotation = rectTransform.rotation; 
        while (t < 1)
        {
            t = (Time.time - timeStarted) / goToPlayerSeconds;

            // if this code doesnt make sense to you then u shouldve paid more attention in ur trig class

            var pos = Vector3.Lerp(startPos, GameManager.playerTransformRef.position, t * t); // t * t so it gets faster (plug x^2 into desmos and look at 0-1 to see the effect for yourself! it will be mind boggling!!!!!)
            var directionToPlayer = (rectTransform.WorldPosition() - GameManager.playerTransformRef.position).normalized;
            var targetRot = Quaternion.LookRotation(directionToPlayer + Vector3.down);
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
        //StaticUtilities.PlayAndDestroyParticle(collectedParticlesPrefab, rectTransform.WorldPosition());
        Destroy(this.gameObject);

        //TODO:
        Debug.Log("TODO: add a confirmation popup");
    }

    #endregion

    #region Data

    private int GetCardHashCode()
    {
        int hash = -1;

        if (dialogueCardDisplay != null)
            hash = dialogueCardDisplay.cardData.GetHashCodeByProperties();

        if (modifierCardDisplay != null)
            hash = modifierCardDisplay.modifierData.GetHashCodeByProperties();

        return HashCode.Combine(hash, startPosition);
    }

    private void OnDestroy()
    {
        // This is to catch cards that may exist in one scene, but not in another
        if(CardPickupManager.Instance != null && !CardPickupManager.Instance.PickupCollectedStatus.ContainsKey(Hash) && !didStart)
        {
            Debug.LogError("Undocumented card is being destroyed! (Did you forget to update the card pickup partent prefab?)");
        }
    }

    #endregion
}
