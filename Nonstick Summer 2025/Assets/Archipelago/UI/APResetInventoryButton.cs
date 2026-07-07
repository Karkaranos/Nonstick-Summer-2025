using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class APResetInventoryButton : MonoBehaviour
{
    [SerializeField, Required] private Button button;
    [SerializeField, Required] private ModifierDeckDisplay modifierDeckDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        APDeckService.Instance.RefreshDeckInventory();
        modifierDeckDisplay.DisplayAllCards(fullReset: true);
    }
}
