using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class EnergyBar : MonoBehaviour
{
    [SerializeField] float animationSpeed = 3;
    private Slider slider;

    public void Initalize()
    {
        slider = slider ?? GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = DialogueManager.MaxEnergy;
        SetValueNoAnimation(DialogueManager.CurrentEnergy);
    }

    public IEnumerator SetValue(float value)
    {
        slider = slider ?? GetComponent<Slider>();
        //if (slider.value == value)
        if (Mathf.Approximately(slider.value, value))
            yield break;

        float oldValue = slider.value;
        while (!Mathf.Approximately(slider.value, value))
        {

            slider.value = Mathf.MoveTowards(slider.value, value, Time.deltaTime * animationSpeed);

            yield return new WaitForEndOfFrame();


            if (slider.value < value)
            {

                yield return new WaitForEndOfFrame();

            }

        }

        slider.value = value;
    }

    public void SetValueNoAnimation(float value)
    {
        slider = slider ?? GetComponent<Slider>();
        //if (slider.value == value)
        if (Mathf.Approximately(slider.value, value))
            return;

        slider.value = value;
    }


    /// <summary>
    /// Updates the UI to match the players energy value 
    /// Coroutine so that it can be animated in the future
    /// </summary>
    /// <returns></returns>
    public IEnumerator Refresh()
    {
        yield return SetValue(DialogueManager.CurrentEnergy);
    }
}
