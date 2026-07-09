using UnityEngine;
using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.LowLevel;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "DialogueBranch", menuName = "Scriptable Objects/DialogueBranch")]
public class DialogueBranch : ScriptableObject
{

    //assign per npc/"moment"?? swap between branches or continue old ones???

    [SerializeField]
    [Label("NPC Dialogue")] public DialogueNPC[] dialogue;

    [SerializeField]
    [BoxGroup("Dialogue Options")][HideIf("End")] public DialogueOption Charming_Expression, Assertive_Expression, Sappy_Expression;
    [BoxGroup("Dialogue Options")][HideIf("End")] public DialogueOption Charming_Question, Assertive_Question, Sappy_Question;

    [SerializeField]
    [BoxGroup("Dialogue Options")] [HideIf("End")] private DialogueOption Silent;

    public bool End;

    [ShowIf("End")] public ArchipelagoLocation BranchLocation;

    private static DialogueOption blank;

    public DialogueOption GetDialogueOption(CardData card)
    {
        if (card == null)
        {
            Debug.Log("Playing silent");
            return Silent ?? blank;
        }

        return GetDialogueOption(card.Emotion, card.Intention);

    }

    public DialogueOption GetDialogueOption(CardEmotion emotion, CardIntention intention)
    {
        switch (intention, emotion)
        {
            case (CardIntention.Expression, CardEmotion.Charming):
                if (Charming_Expression != null)
                {
                    return Charming_Expression;
                }
                else return Silent;
            case (CardIntention.Expression, CardEmotion.Assertive):
                if (Assertive_Expression != null)
                {
                    return Assertive_Expression;
                }
                else return Silent;
            case (CardIntention.Expression, CardEmotion.Sappy):
                if (Sappy_Expression != null)
                {
                    return Sappy_Expression;
                }
                else return Silent;
            case (CardIntention.Question, CardEmotion.Charming):
                if (Charming_Question != null)
                {
                    return Charming_Question;
                }
                else return Silent;
            case (CardIntention.Question, CardEmotion.Assertive):
                if (Assertive_Question != null)
                {
                    return Assertive_Question;
                }
                else return Silent;
            case (CardIntention.Question, CardEmotion.Sappy):
                if (Sappy_Question != null)
                {
                    return Sappy_Question;
                }
                else return Silent;
            default:
                Debug.LogError($"Can't match played card {emotion.ToString()},{intention.ToString()} to result");
                return Silent;
        }

    }


    [Button("Debug Print Best Possible Point Gain")]
    private void PrintBestPointGain()
    {
        var result = _RecurseBestPointGainGreedy(0, new List<string>(), -1);

        Debug.Log($"{result.Item1} possible points (without modifiers)");
        Debug.Log(string.Join(", ", result.Item2));
    }

    private Tuple<float, List<string>> _RecurseBestPointGainGreedy(float currentPointGain, List<string> path, int optionIndex)
    {
        if (End)
            return new Tuple<float, List<string>>(currentPointGain, path);

        List<string> path_copy = new List<string>(path);
        if(optionIndex != -1)
        path_copy.Add(optionIndex.ToString());

        List<DialogueOption> options = new() { Charming_Expression, Assertive_Expression, Sappy_Expression, Charming_Question, Assertive_Question, Sappy_Question, Silent };
        List<DialogueOption> sortedOptions = options.OrderBy(o => o.ChangeInRelationshipStatus).Reverse().ToList();

        // what if they make every option suck for some reason
        float bestPoints = Mathf.NegativeInfinity;
        List<string> best_path = new List<string>(path);

        for (int i = 0; i < 2; i++)
        {
            var option = sortedOptions[i];
            int option_index = options.IndexOf(option);

            Tuple<float, List<string>> result;
            if (option.BranchingDialogueHigh != null)
            {
                result = option.BranchingDialogueHigh._RecurseBestPointGainGreedy(currentPointGain + option.ChangeInRelationshipStatus, path_copy, option_index);
                if (result.Item1 > bestPoints)
                {
                    bestPoints = result.Item1;
                    best_path = result.Item2;
                }
            }

            if (option.RelationshipCheckRequired && option.BranchingDialogueNeutral != null && option.BranchingDialogueNeutral != option.BranchingDialogueLow)
            {
                result = option.BranchingDialogueNeutral._RecurseBestPointGainGreedy(currentPointGain + option.ChangeInRelationshipStatus, path_copy, option_index);
                if (result.Item1 > bestPoints)
                {
                    bestPoints = result.Item1;
                    best_path = result.Item2;
                }
            }

            if (option.RelationshipCheckRequired && option.BranchingDialogueLow != null && option.BranchingDialogueHigh != option.BranchingDialogueLow && option.BranchingDialogueLow != option.BranchingDialogueNeutral)
            {
                result = option.BranchingDialogueLow._RecurseBestPointGainGreedy(currentPointGain + option.ChangeInRelationshipStatus, path_copy, option_index);
                if (result.Item1 > bestPoints)
                {
                    bestPoints = result.Item1;
                    best_path = result.Item2;
                }
            }
        }

        return new Tuple<float, List<string>>(bestPoints, best_path);
    }

    [Button]
    private void DebugPrintCardPreferences()
    {
        _RecurseGetCardPreferences(new Dictionary<string, float>());
    }

    private void _RecurseGetCardPreferences(Dictionary<string, float> results)
    {
        if (End)
        {
            Debug.Log("Card preference result:");
            foreach(KeyValuePair<string, float> pair in results)
            {
                Debug.Log($"{pair.Key}: {pair.Value}");
            }
            return;
        }

        results = new Dictionary<string, float>(results);

        CardEmotion[] emotions = { CardEmotion.Charming, CardEmotion.Sappy, CardEmotion.Assertive };
        CardIntention[] intentions = { CardIntention.Expression, CardIntention.Question };

        List<DialogueOption> options = new() { Charming_Expression, Assertive_Expression, Sappy_Expression, Charming_Question, Assertive_Question, Sappy_Question, Silent };
        HashSet<DialogueBranch> nextBranches = new();

        for (int e = 0; e < emotions.Length; e++)
        {
            for (int i = 0; i < intentions.Length; i++)
            {
                CardEmotion emotion = emotions[e];
                CardIntention intention = intentions[i];
                string card = emotion.ToString() + intention.ToString();

                DialogueOption option = GetDialogueOption(emotion, intention);

                if (!results.ContainsKey(card)) results[card] = 0;
                results[card] += option.ChangeInRelationshipStatus;

                nextBranches.Add(option.BranchingDialogueHigh);
                nextBranches.Add(option.BranchingDialogueNeutral);
                nextBranches.Add(option.BranchingDialogueLow);
            }
        }

        nextBranches.RemoveWhere(b => b == null);

        foreach(DialogueBranch branch in nextBranches)
        {
            branch._RecurseGetCardPreferences(results);
        }
    }

}
