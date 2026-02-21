/*************************************************
* Author Names :          Toby
* Date Created :          7/17/2025
* Updated      :          2/15/2026
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
            .Replace("[Assertive]", $"<sprite name=\"Assertive\"><color=#{CardStyleManager.RedStyle.color.ToHex()}>{CardStyleManager.RedStyle.DisplayName}</color>")
            .Replace("[Charming]", $"<sprite name=\"Charming\"><color=#{CardStyleManager.YellowStyle.color.ToHex()}>{CardStyleManager.YellowStyle.DisplayName}</color>")
            .Replace("[Sappy]", $"<sprite name=\"Sappy\"><color=#{CardStyleManager.BlueStyle.color.ToHex()}>{CardStyleManager.BlueStyle.DisplayName}</color>")
            // intentions
            .Replace("[Expression]", $"<sprite name=\"Statement\"><color=#{CardStyleManager.StatementStyle.color.ToHex()}>{CardStyleManager.StatementStyle.DisplayName}</color>")
            .Replace("[Observation]", $"<sprite name=\"Statement\"><color=#{CardStyleManager.ObservationStyle.color.ToHex()}>{CardStyleManager.StatementStyle.DisplayName}</color>")
            .Replace("[Question]", $"<sprite name=\"Question\"><color=#{CardStyleManager.QuestionStyle.color.ToHex()}>{CardStyleManager.QuestionStyle.DisplayName}</color>")
            .Replace("[Statement]", $"<sprite name=\"Statement\"><color=#{CardStyleManager.StatementStyle.color.ToHex()}>{CardStyleManager.StatementStyle.DisplayName}</color>")
            // specific energy variables
            .Replace("[DrawButtonEnergy]", $"<color=#{GameManager.Instance.NegativeEnergyColor.ToHex()}>- {Mathf.Abs(DialogueManager.DrawButtonEnergyCost)}<sprite name=\"Energy\">energy")
            .Replace("[DiscardEnergy]", $"<color=#{GameManager.Instance.PositiveEnergyColor.ToHex()}>+ {DialogueManager.EnergyGainedPerDiscard}<sprite name=\"Energy\">energy")
            .Replace("[SilentEnergy]", $"<color=#{GameManager.Instance.PositiveEnergyColor.ToHex()}>+ {DialogueManager.EnergyGainedIfSilent}<sprite name=\"Energy\">energy</color>")
            .Replace("[PlayerEnergy]", DialogueManager.CurrentEnergy.ToString())
            // stamps
            .Replace("[Mumble]", $"<sprite name=\"Mumble\"><color=#{GameManager.Instance.StampTooltipColor.ToHex()}>Mumble</color>")
            .Replace("[Repetition]", $"<sprite name=\"Repetition\"><color=#{GameManager.Instance.StampTooltipColor.ToHex()}>Repetition</color>")
            .Replace("[Overthinking]", $"<sprite name=\"Overthinking\"><color=#{GameManager.Instance.StampTooltipColor.ToHex()}>Overthinking</color>")
            .Replace("[Extrovert]", $"<sprite name=\"Extrovert\"><color=#{GameManager.Instance.StampTooltipColor.ToHex()}>Extrovert</color>")
            .Replace("[Confidence]", $"<sprite name=\"Confidence\"><color=#{GameManager.Instance.StampTooltipColor.ToHex()}>Confidence</color>")
            .Replace("[Duplicate Card]", $"<sprite name=\"Duplicate\"><color=#{GameManager.Instance.StampTooltipColor.ToHex()}>Duplicate Card</color>")
            .Replace("[Duplicate]", $"<sprite name=\"Duplicate\"><color=#{GameManager.Instance.StampTooltipColor.ToHex()}>Duplicate</color>")
            .Replace("[Scissors]", $"<sprite name=\"Scissors\"><color=#{GameManager.Instance.StampTooltipColor.ToHex()}>Scissors</color>")
            // people
            .Replace("[Mom]", $"<sprite name=\"Mom Icon\"><color=#{GameManager.Instance.CharactersColor.ToHex()}>Mom</color>")
            .Replace("[Cousin]", $"<sprite name=\"Cousin Icon\"><color=#{GameManager.Instance.CharactersColor.ToHex()}>Cousin</color>")
            .Replace("[Grandma]", $"<sprite name=\"Grandma Icon\"><color=#{GameManager.Instance.CharactersColor.ToHex()}>Grandma</color>")
            .Replace("[Uncle]", $"<sprite name=\"Uncle Icon\"><color=#{GameManager.Instance.CharactersColor.ToHex()}>Uncle</color>")
            // other
            .Replace("[phone]", $"<sprite name=\"Phone\"><color=#{GameManager.Instance.CharactersColor.ToHex()}>phone</color>")
            .Replace("[Social Battery]", $"<sprite name=\"Energy\"><color=#{GameManager.Instance.PositiveEnergyColor.ToHex()}>Social Battery</color>")

            // okay i know its pretty bad to hardcode this but lowkey with such little time left its not worth it to generalize it
            .Replace("[Energy]", $"<color=#{GameManager.Instance.PositiveEnergyColor.ToHex()}>+3 <sprite name=\"Energy\">Energy</color>")

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
