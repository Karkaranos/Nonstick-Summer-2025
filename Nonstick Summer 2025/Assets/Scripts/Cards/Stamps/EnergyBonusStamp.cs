/*****************************************************************************
* File Name :         EnergyBonusStamp.cs
* Author :            Toby
* Creation Date :     July 5, 2025
*
* Brief Description : Adds extra energy after card is played.
*****************************************************************************/

using UnityEngine;
using static RelationshipAffectorStamp;

[CreateAssetMenu(fileName = "EnergyBonusStamp", menuName = "Scriptable Objects/Stamp/Energy Bonus Stamp")]
public class EnergyBonusStamp : ModifierStamp
{
    public float BonusEnergy = 1;
    protected override void EffectTriggered(CardData affectedCard)
    {
        if (!DialogueManager.PlayerInCombat)
            return;

        DialogueManager.CurrentEnergy += BonusEnergy;
    }
}
