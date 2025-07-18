/*************************************************
* Author Names :          Toby
* Date Created :          7/17/2025
* 
* Brief Description : Inputs raw text and outputs
* a filtered version of that text with stylings.
*   
***************************************************/

using UnityEngine;

public static class TextUtilities
{
    public static string FilterText(string rawText)
    {
        // i hope this is not extremely slow
        return rawText
            // emotions
            .Replace("[Assertive]", $"<color=#{CardStyleManager.AssertiveStyle.color.ToHex()}>{CardStyleManager.AssertiveStyle.DisplayName}</color>")
            .Replace("[Charming]", $"<color=#{CardStyleManager.CharmingStyle.color.ToHex()}>{CardStyleManager.CharmingStyle.DisplayName}</color>")
            .Replace("[Sappy]", $"<color=#{CardStyleManager.SappyStyle.color.ToHex()}>{CardStyleManager.SappyStyle.DisplayName}</color>")
            // intentions
            .Replace("[Expression]", $"<color=#{CardStyleManager.ExpressionStyle.color.ToHex()}>{CardStyleManager.ExpressionStyle.DisplayName}</color>")
            .Replace("[Observation]", $"<color=#{CardStyleManager.ObservationStyle.color.ToHex()}>{CardStyleManager.ObservationStyle.DisplayName}</color>")
            .Replace("[Question]", $"<color=#{CardStyleManager.QuestionStyle.color.ToHex()}>{CardStyleManager.QuestionStyle.DisplayName}</color>")
            // specific variables
            .Replace("[DrawButtonEnergy]", (-DialogueManager.DrawButtonEnergyCost).AddSignToString(additonalText: " energy"))
            .Replace("[DiscardEnergy]", DialogueManager.EnergyGainedPerDiscard.AddSignToString(additonalText: " energy"))
            .Replace("[PlayerEnergy]", DialogueManager.CurrentEnergy.ToString())
            // Text "functions"
            .ReplaceTagColor("Stamp", GameManager.Instance.StampTooltipColor.ToHex())
            .ReplaceTagColor("Gray", Color.gray.ToHex())
            .ReplaceTagColor("EnergyColor", GameManager.Instance.PositiveEnergyColor.ToHex());
    }

    /// <summary>
    /// ex: 
    /// 1  -> +1
    /// -2 -> -2
    /// 0  ->  0
    /// </summary>
    public static string AddSignToString(this float input, bool colorByPolarity=true, string additonalText="")
    {
        if(!colorByPolarity)
        {
            if (input <= 0)
                return $"{input.ToString()} {additonalText}";

            return $"+{input.ToString()} {additonalText}";
        }

        if (input > 0)
            return $"<color=#{GameManager.Instance.PositiveEnergyColor.ToHex()}>{input.ToString()} {additonalText}</color>";
        else if (input < 0)
            return $"<color=#{GameManager.Instance.NegativeEnergyColor.ToHex()}>{input.ToString()} {additonalText}</color>";
        else
            return $"<color=#{GameManager.Instance.NeutralEnergyColor.ToHex()}>{input.ToString()} {additonalText}</color>";
    }

    /// <summary>
    /// made specificially for tooltips. honestly, not super universal. but it NEEDED to be in
    /// a static class.
    /// thank you, the internet.
    /// </summary>
    public static string ReplaceTagColor(this string str, string tag, string hex)
    {
        if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(tag) || string.IsNullOrEmpty(hex))
            return str;

        if (hex[0] != '#')
            hex = '#' + hex;

        string startTag = $"[{tag}(";
        string endTag = ")]";

        int currentIndex = 0;
        var result = new System.Text.StringBuilder();

        while (currentIndex < str.Length)
        {
            int startIdx = str.IndexOf(startTag, currentIndex);
            if (startIdx == -1)
            {
                result.Append(str.Substring(currentIndex));
                break;
            }

            result.Append(str.Substring(currentIndex, startIdx - currentIndex));

            int contentStartIdx = startIdx + startTag.Length;
            int endIdx = str.IndexOf(endTag, contentStartIdx);
            if (endIdx == -1)
            {
                result.Append(str.Substring(startIdx));
                break;
            }

            string content = str.Substring(contentStartIdx, endIdx - contentStartIdx);
            result.Append($"<color={hex}>{content}</color>");

            currentIndex = endIdx + endTag.Length;
        }

        return result.ToString();
    }
}
