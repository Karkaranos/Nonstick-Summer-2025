using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class APMomentButton : MonoBehaviour
{
    private const string DEFAULT_CHARACTER_COLOR = "#7B7B7B";
    private const string LOCKED_CHARACTER_COLOR = "#7B7B7B";

    [Header("Archipelago Configuration")]
    [SerializeField] ArchipelagoItem item;
    [SerializeField] private int momentID;
    [SerializeField,Scene] private string sceneToLoad;

    [Header("Character Items")]
    [SerializeField] private ArchipelagoItem mom_item;
    [SerializeField] private ArchipelagoItem grandma_item;
    [SerializeField, HideIf(nameof(_isMoment1))] private ArchipelagoItem cousin_item;
    [SerializeField, HideIf(nameof(_isMoment1))] private ArchipelagoItem uncle_item;

    [Header("Components")]
    [SerializeField, Required] private Button button;
    [SerializeField, Required] private TMP_Text relationshipText;

    private bool _isMoment1 => momentID == 1;

    private void Start()
    {
        ArchipelagoManager.Instance.OnArchipelagoConnected.AddListener(RefreshButtonDisplay);
        RefreshButtonDisplay();

        button.onClick.AddListener(OnButtonPressed);
    }

    private void OnButtonPressed()
    {
        bool momentUnlocked = APInventoryService.Instance.IsItemCollected(item);

        if(momentUnlocked)
            SceneManager.LoadScene(sceneToLoad);
    }

    public void RefreshButtonDisplay()
    {
        bool momentUnlocked = APInventoryService.Instance.IsItemCollected(item);
        
        button.interactable = momentUnlocked;

        BuildRelationshipDisplay(momentUnlocked);
    }

    private void BuildRelationshipDisplay(bool momentUnlocked)
    {
        string output = "";
        var relationshipStatus = APSaveDataService.Instance.GetRelationshipStats(momentID);

        bool mom_unlocked = APInventoryService.Instance.IsItemCollected(mom_item);
        string color_hex = mom_unlocked ? DEFAULT_CHARACTER_COLOR : LOCKED_CHARACTER_COLOR;
        // todo: this can be a lil more robust. i think im leaving it like this for now
        if (mom_unlocked)
            output += $"<sprite name=\"Heart\"><color={DEFAULT_CHARACTER_COLOR}>Mom: {relationshipStatus.MomRelationship.ToString()}";
        else
            output += $"<sprite name=\"Empty Heart\"><color={LOCKED_CHARACTER_COLOR}>Mom: ---";

        bool grandma_unlocked = APInventoryService.Instance.IsItemCollected(grandma_item);
        color_hex = grandma_unlocked ? DEFAULT_CHARACTER_COLOR : LOCKED_CHARACTER_COLOR;
        // todo: this can be a lil more robust. i think im leaving it like this for now
        if (mom_unlocked)
            output += $"\n<sprite name=\"Heart\"><color={DEFAULT_CHARACTER_COLOR}>Grandma: {relationshipStatus.GrandmaRelationship.ToString()}";
        else
            output += $"\n<sprite name=\"Empty Heart\"><color={LOCKED_CHARACTER_COLOR}>Grandma: ---";

        relationshipText.text = output;
    }
}
