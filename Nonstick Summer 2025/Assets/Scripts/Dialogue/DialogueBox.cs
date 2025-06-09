using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{

    private TMP_Text npcText;

    public void Initialize(DialogueBranch branch)
    {

        npcText.text = branch.dialogue[0].Dialogue;

    }

}
