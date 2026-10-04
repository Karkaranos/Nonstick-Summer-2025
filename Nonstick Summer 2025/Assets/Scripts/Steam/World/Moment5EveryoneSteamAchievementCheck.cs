/********************************************************************
* File Name:        Moment5EveryoneSteamAchievementCheck.cs
* Author :          Toby
* Creation Date :   9 / 24 / 2026(day before steam release)
*
*Brief Description: Checks if everyones there in moment 5 and gives achievement
* 
*****************************************************************************/

using NaughtyAttributes;
using UnityEngine;

public class Moment5EveryoneSteamAchievementCheck : MonoBehaviour
{
    [SerializeField] SteamAchievement achievement;
    [SerializeField, Required] private GameObject mom;
    [SerializeField, Required] private GameObject grandma;
    [SerializeField, Required] private GameObject cousin;
    [SerializeField, Required] private GameObject uncle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<PlayerMovement>() == null) return;

        //its the player

        if(mom.gameObject != null && grandma.gameObject != null && cousin.gameObject != null && uncle.gameObject != null)
        {
            SteamAchievementManager.Instance.UnlockAchievement(SteamAchievement.EveryoneMoment5);
        }
    }
}
