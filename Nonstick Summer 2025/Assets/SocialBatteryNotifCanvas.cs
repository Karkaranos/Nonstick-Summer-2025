using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class SocialBatteryNotifCanvas : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField, Tooltip("0 for none, 100 for all black")] private float backgroundDarkeningPercent;
    [SerializeField] private float timeBetweenSteps = .01f;
    [SerializeField] private GameObject button;
    [SerializeField] private TMP_Text text;

    [SerializeField] private string preEmotionText = "";
    [SerializeField] private string postEmotionText = "Cards now cost one Social Battery point to play!";

    private CardEmotion emotion;

    public void Initialize(CardEmotion emotion, Material background)
    {
        this.emotion = emotion;
        backgroundImage.material = background;
        text.text = "";
        button.SetActive(false);
        StartCoroutine(Darken());
    }

    private IEnumerator Darken()
    {
        Color lastColor = backgroundImage.color;
        for(int i=0; i<backgroundDarkeningPercent; i++)
        {
            yield return new WaitForSeconds(timeBetweenSteps);
            backgroundImage.color = new Color(lastColor.r - .01f, lastColor.g - .01f, lastColor.b - .01f);
            lastColor = backgroundImage.color;
        }
        MakeTextAppear();
    }

    private void MakeTextAppear()
    {
        button.SetActive(true);
        text.text = TextUtilities.FilterText(preEmotionText + ColorCard(emotion) + postEmotionText);
    }

    public void CloseMenu()
    {
        UITransitionManager.CloseMenu(changeCam: false);
        MusicManager.instance.StartHouse();
    }

    private string ColorCard(CardEmotion emotion)
    {
        switch (emotion)
        {
            case CardEmotion.Charming:
                return "[Charming] ";
            case CardEmotion.Assertive:
                return "[Assertive] ";
            case CardEmotion.Sappy:
                return "[Sappy] ";
            default:
                return "ERROR";
        }
    }
}
