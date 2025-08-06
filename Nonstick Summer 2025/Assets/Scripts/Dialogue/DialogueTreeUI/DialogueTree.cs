/*****************************************************************************
* File Name :         DialogueTree.cs
* Author :            Jay
* Creation Date :     June 17, 2025
*
* Brief Description :  This should render a dialogue tree on runtime based on an NPC's starting branch.
* If there was a less complicated way to go about this. Whoops.
* This should go on the node prefab itself?
* 
* NOTE: A node should be in scene that the orthographic camera is pointing at. This is where the tree will generate from.
*
* 
*****************************************************************************/

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using NaughtyAttributes;

public class DialogueTree : MonoBehaviour
    
{
    [SerializeField] [Required] [Tooltip("Node prefab goes here!")] private GameObject node;

    Material defaultMaterial;
    [SerializeField][Required][Tooltip("Insert the material for a highlighted node here!")] Material highlightedNodeMaterial;

    [SerializeField][BoxGroup("Line Renderers")] LineRenderer[] lines;

    [Header("Offsets")]

    [SerializeField] float xOffset;
    [SerializeField] float yOffset;

    DialogueBranch newBranch;
    GameObject currentlyHighlightedNode;

    List<DialogueBranch> nodes = new List<DialogueBranch>();
    List<GameObject> nodeVisuals = new List<GameObject>();

    public void Initialize(DialogueBranch branch)
    {

        defaultMaterial = GetComponentInChildren<MeshRenderer>().material;
        GenerateNodes(branch);

        currentlyHighlightedNode = this.gameObject;
        this.GetComponentInChildren<MeshRenderer>().material = highlightedNodeMaterial;

    }

    /// <summary>
    /// continuously spawns nodes until the end of a tree is reached
    /// </summary>
    /// <param name="branch">the current dialogue branch that the player is on</param>
    public void GenerateNodes (DialogueBranch branch, HashSet<DialogueBranch> visitedNodes = null)
    {
        visitedNodes = visitedNodes ?? new HashSet<DialogueBranch>();

        if (visitedNodes.Contains(branch))
        {
            Debug.LogError("Dialogue Branch loops into itself?");
            return;
        }
        visitedNodes.Add(branch);

        //ik we're changing this soon but i can get there when we get there

        DialogueOption[] optionsList = new DialogueOption[9];

        optionsList[0] = branch.Charming_Expression; optionsList[1] = branch.Charming_Observation; optionsList[2] = branch.Charming_Question;
        optionsList[3] = branch.Assertive_Expression; optionsList[4] = branch.Assertive_Observation; optionsList[5] = branch.Assertive_Question;
        optionsList[6] = branch.Sappy_Expression; optionsList[7] = branch.Sappy_Observation; optionsList[8] = branch.Sappy_Question;

        foreach (DialogueOption option in optionsList)
        {

            //there's for sure a better way to do this but i'm not even sure if we're bringing dialogue tree visualization back after recent meetings

            if(option.BranchingDialogueHigh != null)
            {
                newBranch = option.BranchingDialogueHigh;
            }
            if (!nodes.Contains(newBranch))
            {

                nodes.Add(newBranch);

            }
            if (option.BranchingDialogueNeutral != null)
            {
                newBranch = option.BranchingDialogueNeutral;
            }
            if (!nodes.Contains(newBranch))
            {

                nodes.Add(newBranch);

            }
            if (option.BranchingDialogueLow != null)
            {
                newBranch = option.BranchingDialogueLow;
            }
            if (!nodes.Contains(newBranch))
            {

                nodes.Add(newBranch);

            }

        }

        //instantiating nodes at certain positions based on # of nodes so that the tree can look relatively neat i think

        if(nodes.Count == 1)
        {
            //node 1
            GameObject newNode = Instantiate(node);
            newNode.transform.SetParent(this.transform.GetComponentInParent<RectTransform>(), false);
            newNode.transform.position = new Vector3(this.transform.position.x + xOffset, this.transform.position.y, this.transform.position.z);

            lines[0].SetPosition(0, this.transform.position);
            lines[0].SetPosition(1, newNode.transform.position);

            nodeVisuals.Add(newNode);

        }
        else if (nodes.Count == 2)
        {
            //node 1
            GameObject newNode;
            for(int y = 1; y >= -1; y--)
            {
                if (y == 0)
                    continue; // fuck it

                newNode = Instantiate(node);
                newNode.transform.SetParent(this.transform.GetComponentInParent<RectTransform>(), false);
                newNode.transform.position = new Vector3(this.transform.position.x + xOffset, this.transform.position.y + (yOffset * y), this.transform.position.z);

                lines[y + 1].SetPosition(0, this.transform.position);
                lines[y + 1].SetPosition(1, newNode.transform.position);

                nodeVisuals.Add(newNode);
            }

        }
        else if (nodes.Count == 3)
        {
            //node 1
            GameObject newNode;
            for (int y = 1; y >= -1; y--)
            {
                newNode = Instantiate(node);
                newNode.transform.SetParent(this.transform.GetComponentInParent<RectTransform>(), false);
                newNode.transform.position = new Vector3(this.transform.position.x + xOffset, this.transform.position.y + (yOffset * y), this.transform.position.z);

                lines[y+1].SetPosition(0, this.transform.position);
                lines[y+1].SetPosition(1, newNode.transform.position);

                nodeVisuals.Add(newNode);
            }
        }

        for(int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] != null && !nodes[i].End)
            {
                var dt = nodeVisuals[i].GetComponent<DialogueTree>();
                if(dt!= null)
                {
                    dt.GenerateNodes(nodes[i],visitedNodes);
                }
            }
        }

    }

    /// <summary>
    /// highlights the node representing where the player is at in a dialogue tree with a character
    /// TO DO: disable tree for side npcs who aren't acting as bosses??
    /// </summary>
    /// <param name="branch">the current dialogue branch that the player is on</param>
    public void HighlightActiveNode(DialogueBranch branch)
    {

        if(currentlyHighlightedNode != null)
        {

            currentlyHighlightedNode.GetComponentInChildren<MeshRenderer>().material = defaultMaterial;

        }

        currentlyHighlightedNode.GetComponent<DialogueTree>().nodeVisuals
            [currentlyHighlightedNode.GetComponent<DialogueTree>().nodes.IndexOf(branch)].GetComponentInChildren<MeshRenderer>().material
            = highlightedNodeMaterial;

        currentlyHighlightedNode = currentlyHighlightedNode.GetComponent<DialogueTree>().nodeVisuals
            [currentlyHighlightedNode.GetComponent<DialogueTree>().nodes.IndexOf(branch)];

    }

}
