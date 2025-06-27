/*****************************************************************************
* File Name :         OpenPause.cs
* Author :            Cade, Toby
* Creation Date :     June ?, 2025
* Updated :           June 26, 2025
*
* Brief Description : adds listeners to open the pause menu, and the inventory menu.
* 
*****************************************************************************/

using UnityEngine;
using NaughtyAttributes;

public class OpenPause : MonoBehaviour
{
    [SerializeField, Required] public GameObject PauseMenu;
    [SerializeField, Required] public GameObject InventoryMenuPrefab;

    /// <summary>
    /// Occurs at the first frame. Initializes events
    /// </summary>
    private void Start()
    {
        InputEvents.PauseStarted.AddListener(PausePressed);
        InputEvents.InventoryStarted.AddListener(InventoryPressed);
    }

    /// <summary>
    /// Handles opening or closing the pause menu 
    /// </summary>
    public void PausePressed()
    {
        if (UITransitionManager.PlayerInMenu)
        {
            // close pause or inventory menu if those are open.
            var currentMenu = UITransitionManager.CurrentCanvasReference;
            if (currentMenu!=null && (currentMenu.GetComponent<PauseAndSettings>() || currentMenu.GetComponent<ModifierInventoy>()))
                UITransitionManager.CloseMenu();

            return;
        }
        // Prevents the player from pausing while in combat. can be revisited later
        UITransitionManager.OpenMenuIfNoOtherMenusAreOpenRightNow(PauseMenu, out GameObject _);

        /* sorry i made a new function :P
         * else if (UITransitionManager.CurrentCanvasReference != null &&
            UITransitionManager.CurrentCanvasReference.GetComponent<DialogueUIController>())
        {
            Debug.LogWarning("Pausing is prohibited during combat");
        }
        else
        {
            UITransitionManager.OpenMenu(PauseMenu);
        }*/
    }

    /// <summary>
    /// Opens player inventory iff no other menus open
    /// </summary>
    private void InventoryPressed()
    {
        if (UITransitionManager.PlayerInMenu &&
            UITransitionManager.CurrentCanvasReference.GetComponent<ModifierInventoy>() != null)
        {
            UITransitionManager.CloseMenu();
            return;
        }

        if(UITransitionManager.PlayerInMenu)
        {
            return;
        }

        UITransitionManager.OpenMenuIfNoOtherMenusAreOpenRightNow(InventoryMenuPrefab, out GameObject _);
    }
}
