/*****************************************************************************
* File Name :         DialogueTree.cs
* Author :            Jay
* Creation Date :     June 17, 2025
*
* Brief Description :  This should render a dialogue tree on runtime based on an NPC's starting branch.
* If there was a less complicated way to go about this. Whoops.
* This should go on the node prefab itself?
* 
*****************************************************************************/

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueTree : MonoBehaviour
    
{
    [SerializeField] [Tooltip("Node prefab goes here!")] private GameObject node;

    public void GenerateNodes (DialogueBranch branch)
    {

        List<DialogueBranch> nodes = new List<DialogueBranch>();

        foreach (DialogueOption option in branch.GetComponents<DialogueOption>())
        {

            DialogueBranch newBranch = option.BranchingDialogue;
            if (!nodes.Contains(newBranch))
            {

                nodes.Add(newBranch);

            }

        }
        for (int i = 0; i < nodes.Count; i++)
        {

            float offset = i * 1;
            Vector3 pos = this.gameObject.transform.position + transform.up * offset;
            GameObject newNode = Instantiate(node, pos, transform.rotation);

            if (!nodes[i].End)
            {

                newNode.GetComponent<DialogueTree>().GenerateNodes(nodes[i]);

            }

        }

    }
}
