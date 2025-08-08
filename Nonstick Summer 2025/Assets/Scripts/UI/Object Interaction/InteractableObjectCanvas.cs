/*************************************************
Author Names :          Toby, Cade
Date Created :          ??
Date Modified :         June 19, 2025
Brief Description :     Handles functionality for interactable objects
                        Assigns buttons and gets the player's choice
                        Yields cards once
***************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InteractableObjectCanvas : MonoBehaviour
{

    [Tooltip("Can be left null if you don't want the camera to move.")]
    [SerializeField]
    private Transform cameraAnchor;

    [SerializeField]
    [Required]
    private Button Button1;

    [SerializeField]
    [Required]
    private Button Button2;

    [SerializeField]
    [Required]
    private Button Button3;

    [SerializeField] [Required] private TMP_Text statementField;
    [SerializeField] [Required] private TMP_Text questionField;

    [SerializeField] [Required] private GameObject ModifierObtainCanvas;

    private GameObject objectRef;
    private PersonalityOption[] options;

    [SerializeField] private GameObject interactionButtons;
    [SerializeField] private GameObject otherButtons;

    private char[] Punctuation = { '!', '.' };

    public void Initialize(string statement, string question, PersonalityOption[] options, GameObject g = null)
    {
        otherButtons.SetActive(false);
        interactionButtons.SetActive(true);

        this.options = options;
        Button1.onClick.AddListener(() => OnClickInteractableObject(options[0]));
        Button2.onClick.AddListener(() => OnClickInteractableObject(options[1]));
        Button3.onClick.AddListener(() => OnClickInteractableObject(options[2]));

        statementField.text = statement;
        questionField.text = question;

        Button1.image.color = options[0].ButtonColor;
        Button2.image.color = options[1].ButtonColor;
        Button3.image.color = options[2].ButtonColor;

        if (g != null)
            objectRef = g;

        TMP_Text button1Text = Button1.GetComponentInChildren<TMP_Text>();
        button1Text.text = options[0].ButtonText;

        TMP_Text button2Text = Button2.GetComponentInChildren<TMP_Text>();
        button2Text.text = options[1].ButtonText;

        TMP_Text button3Text = Button3.GetComponentInChildren<TMP_Text>();
        button3Text.text = options[2].ButtonText;

    }


    public void InitializeWithBlocker(string statement, string response)
    {
        otherButtons.SetActive(true);
        interactionButtons.SetActive(false);

        statementField.text = statement;
        questionField.text = response;
    }

    public void InitializeAfterModifier(string statement, string response1, string response2, string choice)
    {
        otherButtons.SetActive(true);
        interactionButtons.SetActive(false);

        statementField.text = statement;

        // The reason I'm using LastIndex here is because punctuation is pretty much only at the end of choices. 
        // It does not scale, and I know that, but it should serve its purpose given how much time we have left in the project
        if(choice.LastIndexOfAny(Punctuation)!=-1)
        {
            choice = choice.Remove(choice.LastIndexOfAny(Punctuation));
        }

        questionField.text = response1 + " " + choice + " " + response2;
    }

    /// <summary>
    /// Gives player modifier cards based on their emotion choice
    /// </summary>
    /// <param name="emotion">The chosen emotion</param>
    public void OnClickInteractableObject(PersonalityOption PO)
    {
        foreach (ModifierData md in PO.ModifiersToGive)
        {
            var iopc = UITransitionManager.OpenMenu(ModifierObtainCanvas, cameraAnchor).GetComponent<ItemObtainPopupCanvas>();
            iopc.Initialize(md);
            ModifierManager.AddCard(md, true);
        }
        if (objectRef != null)
        {
            foreach(PersonalityOption po in options)
            {
                if(po == PO)
                {
                    objectRef.GetComponent<InteractableObjectBehavior>().chosenOption = po.ButtonText;
                }
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (cameraAnchor == null)
            return;

        if (!StaticUtilities.Editor_SelectingSelfOrChild(this.transform))
            return;

        Gizmos.color = Color.blue; // blue becuase the unity camera icon color is blue
        Gizmos.DrawRay(cameraAnchor.position, cameraAnchor.forward);
        Gizmos.DrawWireSphere(cameraAnchor.position, 0.25f);
    }
#endif
}
