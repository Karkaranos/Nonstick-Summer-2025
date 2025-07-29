using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class EnergyBar : MonoBehaviour
{
    [SerializeField] float animationSpeed = 3;
    private Slider slider;

    private TMP_Text energyNumber;

    public void Initalize()
    {
        slider = slider ?? GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = DialogueManager.MaxEnergy;
        SetValueNoAnimation(DialogueManager.CurrentEnergy);

        energyNumber = GetComponentInChildren<TMP_Text>();
        energyNumber.text = DialogueManager.CurrentEnergy.ToString();

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

            energyNumber.text = value.ToString();

            yield return new WaitForEndOfFrame();
        }

        slider.value = value;

        energyNumber.text = value.ToString();

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
