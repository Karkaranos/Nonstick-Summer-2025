using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class APConnectionMenu : MonoBehaviour
{
    [SerializeField, Required] private TMP_InputField serverUrlField;
    [SerializeField, Required] private TMP_InputField slotNameField;
    [SerializeField, Required] private TMP_InputField passwordField;

    [SerializeField, Required] private TMP_Text connectionStateText;
    [SerializeField, Required] private Button connectButton;

    [SerializeField] private string SuccessfulConnectionText;
    [SerializeField] private string UnsuccessfulConnectionText;


    [SerializeField, Required] private Image iconImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serverUrlField.text = ArchipelagoManager.Instance.serverUrl;
        slotNameField.text = ArchipelagoManager.Instance.slotName;
        passwordField.text = ArchipelagoManager.Instance.password;

        connectionStateText.text = UnsuccessfulConnectionText;

        iconImage.color = Color.gray;

        connectButton.onClick.AddListener(OnConnectButtonPressed);  
    }

    void OnConnectButtonPressed()
    {
        ArchipelagoManager.Instance.serverUrl = serverUrlField.text;
        ArchipelagoManager.Instance.slotName  = slotNameField.text;
        ArchipelagoManager.Instance.password  = passwordField.text;

        // Do i need to await this?
        ArchipelagoManager.Instance.ConnectToArchipelago();

        connectionStateText.text = ArchipelagoManager.Instance.isConnected ?
            SuccessfulConnectionText : 
            UnsuccessfulConnectionText;

        iconImage.color = ArchipelagoManager.Instance.isConnected ?
            Color.white :
            Color.gray;
    }
}
