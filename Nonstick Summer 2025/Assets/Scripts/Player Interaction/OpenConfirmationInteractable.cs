using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************
// File Name :          OpenConfirmationInteractable.cs
// Author :             Sky
// Creation Date :      July 10, 2025
// Modified Date :      July 10, 2025
//
// Brief Description :  Controls confirmation menu popups through interaction.

*****************************************************************************/
public class OpenConfirmationInteractable : MonoBehaviour, IInteractable
{

    [Header("Shaders")]
    [SerializeField, Tooltip("Indicates this object is used in Objectives")] private bool applyLevelEndShader;
    [ShowIf("applyLevelEndShader"), SerializeField] private Material levelEndShader;
    [ShowIf("applyLevelEndShader"), SerializeField, Tooltip("GameObjects to affect")] private GameObject[] affectedMeshes;

    [Header("Scene Transition")]
    [HideInInspector] public bool InteractSuccessful = false;
    [HideInInspector] public bool PlayerCanLeave = false;
    [SerializeField, Scene] public int NextSceneIndex;
    [SerializeField, Tooltip("Which object moves you between scenes.")] BedInteractionPopupCanvas.EndType sceneTransitionType;

    [SerializeField]
    [Required]
    public GameObject CanvasToOpenPrefab;

    [Tooltip("Can be left null if you don't want the camera to move.")]
    [SerializeField]
    private Transform cameraAnchor;

    [Header("Archipelago")]
    public ArchipelagoLocation APLocation;

    private void Start()
    {
        if(APLocation == ArchipelagoLocation.None)
        {
            Debug.LogError($"{gameObject.name}s ap location is None");
        }

        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains('5'))
        {
            PlayerCanLeave = true;
            if (applyLevelEndShader)
            {
                SetShader();
            }
        }
    }

    public void Interact(GameObject player)
    {
        var menu = UITransitionManager.OpenMenu(CanvasToOpenPrefab, cameraAnchor, gameObject);
        menu.GetComponent<BedInteractionPopupCanvas>().APLocation = this.APLocation;
        menu.GetComponent<BedInteractionPopupCanvas>().Bed = this;
        menu.GetComponent<BedInteractionPopupCanvas>().SceneTransitionType = sceneTransitionType;
    }

    public void ClearBlocker()
    {
        PlayerCanLeave = true;
        if(applyLevelEndShader)
        {
            SetShader();
        }
    }

    public void SetShader()
    {
        foreach (GameObject g in affectedMeshes)
        {
            Renderer mr = g.GetComponent<Renderer>();
            List<Material> allMats = new List<Material>();
            foreach (Material m in mr.materials)
            {
                allMats.Add(m);
            }
            if (applyLevelEndShader && levelEndShader != null)
                allMats.Add(levelEndShader);
            mr.SetMaterials(allMats);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (cameraAnchor == null)
            return;

        if (!StaticUtilities.Editor_SelectingSelfOrChild(this.transform))
            return;

        Gizmos.color = Color.blue; // blue becuase the unity camera icon color is blue
        Gizmos.DrawRay(cameraAnchor.position, cameraAnchor.forward);
        Gizmos.DrawWireSphere(cameraAnchor.position, 0.25f);
    }
#endif
}
