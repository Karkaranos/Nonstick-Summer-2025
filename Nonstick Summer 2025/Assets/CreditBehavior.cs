/*****************************************************************************
* File Name :         CreditBehavior.cs
* Author :            Cade
* Creation Date :     8/12/1015
*
* Brief Description : Handles the game's credits
* 
*****************************************************************************/
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
public class CreditBehavior : MonoBehaviour
{
    [SerializeField] private GameObject creditParent;
    [SerializeField] private GameObject creditScroll;
    [SerializeField] private float creditSpeed;
    private float heightToReach = 7200;
    [SerializeField] private float pauseBeforeStartEnd = 1.5f;

    [SerializeField] [Required] private Image creditsFadeToBlack;


    [SerializeField, ReadOnly] Vector3 creditStart;
    private Coroutine credits;

    [SerializeField] [Required] private RectTransform creditsEndingFrame;
    [SerializeField] private GameObject button;

    private MainMenu mm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mm = FindFirstObjectByType<MainMenu>(FindObjectsInactive.Include);
        creditStart = creditScroll.transform.localPosition;
        heightToReach = creditScroll.GetComponent<RectTransform>().rect.height;
    }

    public void OpenCredits(bool buttonActive)
    {
        button.SetActive(buttonActive);
        creditParent.SetActive(true);
        credits = StartCoroutine(ScrollCredits());
    }
    private IEnumerator ScrollCredits()
    {
        creditsFadeToBlack.color = Color.clear;

        yield return new WaitForSeconds(pauseBeforeStartEnd);
        Vector3 pos = creditScroll.transform.position;
        Debug.Log(creditsEndingFrame.position);
        while (creditsEndingFrame.position.y < Screen.height / 2)
        {
            //i'm adding more
            if (Input.GetKey(KeyCode.Escape))
            {
                mm.CloseCredits();
                yield break;
            }
            // old input system lol
            float speedUp = (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Space)) ? 8 : 1;

            pos.y += creditSpeed * Time.deltaTime * Mathf.Clamp(Screen.height / 1280, 1, 3) * speedUp;
            creditScroll.transform.position = pos;
            yield return null;
        }
        yield return new WaitForSeconds(pauseBeforeStartEnd);

        float timeElapsed = 0;
        while (timeElapsed < 2)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / 2;
            creditsFadeToBlack.color = new Color(0, 0, 0, t);
            yield return null;
        }

        yield return new WaitForSeconds(pauseBeforeStartEnd * 2);


        if (mm != null)
        {
            mm.CloseCredits();
        }
        else
        {
            CloseCredits();
        }

    }

    public void CloseCredits()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ResetCredits()
    {
        StopCoroutine(credits);
        creditScroll.transform.localPosition = creditStart;
    }

}


