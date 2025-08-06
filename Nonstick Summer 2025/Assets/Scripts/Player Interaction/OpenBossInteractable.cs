using NaughtyAttributes;
using UnityEngine;
/*****************************************************************************
* File Name :         OpenBossInteractable.cs
* Author :            Sky, Cade
* Creation Date :     June 18, 2025
*
* Brief Description : Opens combat automatically for bosses on trigger enter.
* Previously, inherited from IInteractable, and required player input.
*
* TODO:
* 
* 
*****************************************************************************/
public class OpenBossInteractable : MonoBehaviour
{
    [SerializeField] bool _isMoment4 = false;
    [ShowIf("_isMoment4"), Tooltip("Interacting with this object will make the boss appear"), SerializeField] private GameObject _bossAppearItem;
    [Required]
    public GameObject CanvasToOpenPrefab;

    [SerializeField]
    [Required]
    private DialogueBranch StartingDialogueBranch;

    [Tooltip("Can be left null if you don't want the camera to move.")]
    [SerializeField]
    private Transform cameraAnchor;

    [Tooltip("Current character you're interacting with.")]
    [SerializeField]
    private characters character;

    private void Start()
    {
        if(_isMoment4)
        {
            gameObject.SetActive(false);
        }
    }

    public void TryActivatingBoss(GameObject obj)
    {
        print("Called");
        if(!_isMoment4)
        {
            return;
        }
        if(obj == _bossAppearItem)
        {
            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// opens combat on trigger enter.
    /// </summary>
    public void OpenCanvas()
    {
        float whichBoss = 0;
        if (character == characters.Grandma)
        {
            whichBoss = 3;
        }
        else if (character == characters.Mom)
        {
            whichBoss = 1;
        }
        else if (character == characters.Cousin)
        {
            whichBoss = 2;
        }
        MusicManager.instance.StartCombat(whichBoss);

        var menu = UITransitionManager.OpenMenu(CanvasToOpenPrefab, cameraAnchor);
        GameManager.ObjectiveReference.SetObjectiveVisibility(false);
        StartCoroutine(DialogueUIController.Instance.Initialize(StartingDialogueBranch, character, true, gameObject));
    }


    /// <summary>
    /// checking for trigger activation for auto boss interactions
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) // moved from interact.cs
    {
        if (other.GetComponent<PlayerMovement>())
        {
            OpenCanvas();
            Destroy(gameObject.GetComponent<Collider>());
        }
    }

    private void OnDrawGizmos()
    {
        if (cameraAnchor == null)
            return;

        if (!StaticUtilities.Editor_SelectingSelfOrChild(this.transform))
            return;

        Gizmos.color = Color.blue; // blue because the unity camera icon color is blue
        Gizmos.DrawRay(cameraAnchor.position, cameraAnchor.forward);
        Gizmos.DrawWireSphere(cameraAnchor.position, 0.25f);
    }

}
