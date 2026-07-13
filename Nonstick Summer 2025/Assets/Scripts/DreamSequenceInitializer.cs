using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;
/*****************************************************************************
* File Name :         DreamSequence.cs
* Author :            Sky
* Creation Date :     July 11, 2025
*
* Brief Description :  Controls canvas anD actions During Dream Sequence.
* 
*****************************************************************************/
public class DreamSequenceInitializer : MonoBehaviour
{
    [Header("Required Attributes")] [Required]
    public GameObject CanvasToOpen;
    [Tooltip ("Fade from black prefab")]
    [SerializeField] private GameObject fadeFromBlack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DoFadeIn();
        var dream = UITransitionManager.OpenMenu(CanvasToOpen);
        if (dream == null)
            return;

        dream.GetComponent<DreamSequence>().Initialize();
    }


    public void DoFadeIn()
    {
        var canvas = Instantiate(fadeFromBlack);
        canvas.SetActive(true);
        var fade = canvas.GetComponent<FadeTransition>();
        var image = canvas.GetComponentInChildren<Image>();

        if (fade != null)
        {
            fade.StartFadeIn(image);
        }
    }
}