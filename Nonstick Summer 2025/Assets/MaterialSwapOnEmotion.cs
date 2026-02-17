/*****************************************************************************
// File Name :          MaterialSwapOnEmotion.cs
// Author :             Cade
// Creation Date :      February 17, 2026
// Modified Date :      February 17, 2026
//
// Brief Description :  Swaps materials based on the emotion chosen at the start of the level

*****************************************************************************/
using UnityEngine;

public class MaterialSwapOnEmotion : MonoBehaviour
{
    public Material SappyColor;
    public Material AssertiveColor;
    public Material CharmingColor;

    public MeshRenderer AffectedRenderer;

    /// <summary>
    /// Sets the provided renderer to the correct emotion
    /// </summary>
    /// <param name="emotion"></param>
    public void SetColor(CardEmotion emotion)
    {
        switch (emotion)
        {
            case CardEmotion.Charming:
                AffectedRenderer.material = CharmingColor;
                break;
            case CardEmotion.Assertive:
                AffectedRenderer.material = AssertiveColor;
                break;
            case CardEmotion.Sappy:
                AffectedRenderer.material = SappyColor;
                break;
            default:
                break;
        }

    }

}
