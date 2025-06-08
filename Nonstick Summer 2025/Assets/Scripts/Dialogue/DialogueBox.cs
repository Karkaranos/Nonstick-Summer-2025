using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
 
    public void UpdateDialogueNPC(string npcText)
    {

        GetComponent<TMP_Text>().text = npcText;

    }

}
