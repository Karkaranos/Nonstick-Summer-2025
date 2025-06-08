using TMPro;
using UnityEngine;

public class ProgessDialogue : MonoBehaviour
{

    [HideInInspector] public DialogueBranch branch;

    [Tooltip("Drop the dialogue box from the UI here!")]
    [SerializeField] DialogueBox box;

    int currentText = 0;

    private void Start()
    {

        box.UpdateDialogueNPC(branch.dialogue[0].Dialogue);

    }

    void ProgressDialogue()
    {

        currentText += 1;
        box.UpdateDialogueNPC(branch.dialogue[currentText].Dialogue);

    }

}
