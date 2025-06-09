using System.Collections;
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
        slider.value = currentValue;
    }

    /// <summary>
    /// Sets new values of slider, updates
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public IEnumerator SetValue(float value)
    {
        slider = slider ?? GetComponent<Slider>();

        if (slider.value == value)
            yield break;

        slider.value = value;
        yield return null; 
        // yield return null instead of yield break because fuck it we ball
        // questionable code is okay if you say "fuck it we ball" btw.
        // ok no but my actual reason for doing this is to make sure the future animation stuff wont break if not everything is happening at the same frame.
        // sorry for making this comment so long

        //thanks toby
    }

    /// <summary>
    /// Updates the UI to match the character's relationship value
    /// Coroutine so that it can be animated in the future
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public IEnumerator Refresh(characters character)
    {
        yield return SetValue(RelationshipManager.characterRelationships[character].currentValue);
    }
}
