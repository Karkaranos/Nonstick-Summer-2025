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
using System;

public class OpenCanvasInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    [Required]
    public GameObject CanvasToOpenPrefab;

    [Tooltip("Can be left null if you don't want the camera to move.")]
    [SerializeField]
    private Transform cameraAnchor;

    private GameObject openedCanvas;

    [Header("Personality Question")]
    [SerializeField] private string _question;
    [SerializeField] private PersonalityOption[] _options = new PersonalityOption[3];
    private int chosenOption = -1;
    private bool hasGivenCard = false;

    /// <summary>
    /// Opens or closes the canvas and handles setting visuals
    /// </summary>
    /// <param name="player"></param>
    public void Interact(GameObject player)
    {
        openedCanvas = UITransitionManager.OpenMenu(CanvasToOpenPrefab, cameraAnchor, gameObject);

        // If this object has given cards, set the card button to false
        if (hasGivenCard)
        {
            openedCanvas.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);

            // Display what the player last chose
            openedCanvas.transform.GetChild(0).transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = _options[chosenOption].ButtonText;
        }
        // If this object has not given cards, set the buttons and assign their on click
        else
        {
            Transform savedObject = openedCanvas.transform.GetChild(0).transform.GetChild(0);
            openedCanvas.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);

            // Set button visuals for each option
            for(int i=0; i<3; i++)
            {
                savedObject.GetChild(i).GetComponent<Image>().color = _options[i].ButtonColor;
                savedObject.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = _options[i].ButtonText;
            }

            // Set button on click references
            // Unity didn't like it when this occured in the loop, hence why it is hardcoded
            savedObject.GetChild(0).GetComponent<Button>().onClick.AddListener(() => CallGiveCard(_options[0].Emotion));
            savedObject.GetChild(1).GetComponent<Button>().onClick.AddListener(() => CallGiveCard(_options[1].Emotion));
            savedObject.GetChild(2).GetComponent<Button>().onClick.AddListener(() => CallGiveCard(_options[2].Emotion));

            savedObject.GetChild(3).GetComponent<TMP_Text>().text = _question;

        }
    }

    /// <summary>
    /// Helper function to streamline assigning onClick. Calls GetEmotion and yields one of each intent
    /// </summary>
    /// <param name="emotion">The emotion to give cards of</param>
    public void CallGiveCard(CardEmotion emotion)
    {
        UIUtilityFunctions.GetEmotion(emotion, gameObject);
    }

    /// <summary>
    /// If giving one card
    /// </summary>
    [Obsolete("This function is no longer needed")]
    public void GiveCard()
    {
        hasGivenCard = true;
    }

    /// <summary>
    /// Ensures the player gets cards only once
    /// Saves the emotion
    /// </summary>
    /// <param name="emotion">The chosen emotion</param>
    public void GiveCard(CardEmotion emotion)
    {
        hasGivenCard = true;

        for(int i=0; i<_options.Length; i++)
        {
            if(_options[i].Emotion == emotion)
            {
                chosenOption = i;
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


[System.Serializable]
/*************************************************
Author Names :          Cade Naylor
Date Created :          June 19, 2025
Date Modified :         June 19, 2025
Brief Description :     Stores information for interactable object questions
                        Toby, if you would rather I switch this to a scriptable object later, I can
                        The reason I'm doing it this way is to slightly optimize project size
***************************************************/
public class PersonalityOption
{
    [Tooltip("Option text")]public string ButtonText;
    [Tooltip("An optional tint for the button. Leave white if not")]public Color ButtonColor = Color.white;
    [Tooltip("The emotion of cards to yield")] public CardEmotion Emotion;

}
