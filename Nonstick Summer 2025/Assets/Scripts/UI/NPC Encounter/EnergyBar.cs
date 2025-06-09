using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class EnergyBar : MonoBehaviour
{
    private Slider slider;

    public void Initalize(int maxValue)
    {
        slider = slider ?? GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = maxValue;
    }

    public IEnumerator SetValue(float value)
    {
        slider = slider ?? GetComponent<Slider>();
        if (slider.value == value)
            yield break;

        slider.value = value;
        yield return null; // yield return null instead of yield break because fuck it we ball
        // questionable code is okay if you say "fuck it we ball" btw.
        // ok no but my actual reason for doing this is to make sure the future animation stuff wont break if not everything is happening at the same frame.
        // sorry for making this comment so long
    }

    public IEnumerator Refresh()
    {
        yield return SetValue(DialogueManager.CurrentEnergy);
    }
}
