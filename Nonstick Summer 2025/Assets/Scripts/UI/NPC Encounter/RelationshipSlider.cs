using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
/*****************************************************************************
* File Name :         RelationshipSlider.cs
* Author :            Sky
* Creation Date :     June 8, 2025
*
* Brief Description : Controls relationship slider for each character. Will update
* visuals when in combat.
*
* TODO:
* 
* 
*****************************************************************************/
public class RelationshipSlider : MonoBehaviour
{
    [Tooltip("Slider that displays the character's current relationship value.")]
    private Slider slider;
    [SerializeField] float animationSpeed = 3;

    [SerializeField] TMP_Text AP_RelationshipDisplay;

    /// <summary>
    /// Initializes slider values and visuals
    /// </summary>
    /// <param name="maxValue"></param>
    /// <param name="currentValue"></param>
    public void Initialize(float maxValue, float currentValue)
    {
        slider = slider ?? GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = maxValue;
        SetValueNoAnimation(currentValue);
    }

    /// <summary>
    /// Sets new values of slider, updates
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public IEnumerator SetValue(float value)
    {
        slider = slider ?? GetComponent<Slider>();
        if (value > slider.maxValue)
        {
            //value = slider.maxValue;
            slider.maxValue = value; // nuh uh
        }
        //if (slider.value == value)
        if (Mathf.Approximately(slider.value, value))
            yield break;

        float oldValue = slider.value;
        while (!Mathf.Approximately(slider.value, value))
        {
            float newValue = Mathf.MoveTowards(slider.value, value, Time.deltaTime * animationSpeed);
            slider.value = newValue;
            AP_RelationshipDisplay.text = (Mathf.Round(newValue*10)/10f).ToString();

            yield return new WaitForEndOfFrame();
        }

        slider.value = value;
        AP_RelationshipDisplay.text = value.ToString();
    }

    public void SetValueNoAnimation(float value)
    {
        slider = slider ?? GetComponent<Slider>();
        //if (slider.value == value)
        if (Mathf.Approximately(slider.value, value))
            return;

        if (value > slider.maxValue)
        {
            value = slider.maxValue;
        }
        slider.value = value;
        AP_RelationshipDisplay.text = value.ToString();
    }

    /// <summary>
    /// Updates the UI to match the character's relationship value
    /// Coroutine so that it can be animated in the future
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public IEnumerator Refresh(Character character)
    {
        yield return SetValue(RelationshipManager.characterRelationships[character].currentValue);
    }

}
