using UnityEngine;
using NaughtyAttributes;

/*
 * Values associated with a cards intention/emotion.
 * Used for card display and tooltips
 */

[System.Serializable]
public class CardValueStyle
{
    public Color color = Color.white;
    public string DisplayName = "EMPTY";
    [ShowAssetPreview(32, 32)]
    public Sprite sprite = null;

    /// <summary>
    /// ideally, this constructor should only be used for debug, like once
    /// </summary>
    public CardValueStyle(Color color, string displayName) {
        this.color = color;
        DisplayName = displayName;
    }

    // font?
    // description?
}
