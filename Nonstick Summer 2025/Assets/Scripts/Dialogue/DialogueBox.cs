using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{

    private TMP_Text npcText;

    public void Initialize(DialogueBranch branch)
    {

        npcText = GetComponent<TMP_Text>();


        npcText.text = branch.dialogue[0].Dialogue;

    }

    public void ProgressDialogue(DialogueBranch branch, int numberInList)
    {

        npcText = GetComponent<TMP_Text>();

        npcText.text = branch.dialogue[numberInList].Dialogue;
        if (branch.dialogue[numberInList].End)
        {

            DialogueUIController.Instance.ClosingOutCombat();
            DialogueUIController.Instance.HideDeck();

        }

    }

}
