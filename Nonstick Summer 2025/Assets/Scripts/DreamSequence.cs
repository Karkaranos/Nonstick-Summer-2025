using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using TMPro;

public class DreamSequence : MonoBehaviour
{
    [SerializeField] [Required] private TMP_Text statementField;
    [SerializeField] [Required] private TMP_Text questionField;

    [SerializeField] [Required] private Button Option1;
    [SerializeField] [Required] private Button Option2;
    [SerializeField] [Required] private Button Option3;

    [SerializeField] private string statement = "You are lost in thought.";
    [SerializeField] private string question = "How do you feel?";

    [Header("Options")]
    [SerializeField] private PersonalityOption[] _options = new PersonalityOption[3];

    void Start()
    {
        statementField.text = statement;
        questionField.text = question;

        TMP_Text textOption1 = Option1.GetComponentInChildren<TMP_Text>();
        textOption1.text = _options[0].ButtonText;

        TMP_Text textOption2 = Option2.GetComponentInChildren<TMP_Text>();
        textOption2.text = _options[1].ButtonText;

        TMP_Text textOption3 = Option3.GetComponentInChildren<TMP_Text>();
        textOption3.text = _options[2].ButtonText;

        Option1.image.color = _options[0].ButtonColor;
        Option2.image.color = _options[1].ButtonColor;
        Option3.image.color = _options[2].ButtonColor;
    }

    public void OnCharmingChosen()
    {
        MoodManager.SetDreamSequenceCost(CardEmotion.Charming);
        UITransitionManager.CloseMenu(changeCam:false);
    }

    public void OnSappyChosen()
    {
        MoodManager.SetDreamSequenceCost(CardEmotion.Sappy);
        UITransitionManager.CloseMenu(changeCam:false);
    }

    public void OnAssertiveChosen()
    {
        MoodManager.SetDreamSequenceCost(CardEmotion.Assertive);
        UITransitionManager.CloseMenu(changeCam:false);
    }


    [System.Serializable]
    /*************************************************
    Author Names :          Cade Naylor
    Date Created :          June 19, 2025
    Date Modified :         June 19, 2025
    Brief Description :     Stores information for interactable object questions
    ***************************************************/
    public class PersonalityOption
    {
        [Tooltip("Option text")] public string ButtonText;
        [Tooltip("An optional tint for the button. Leave white if not")] public Color ButtonColor = Color.white;
    }
}
