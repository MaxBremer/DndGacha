using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ClothOfErasAbility : RangedAttackEnemiesAbility
{
    public ClothOfErasAbility()
    {
        Name = "ClothOfEras";
        DisplayName = "Cloth of Eras";
        MaxCooldown = 0;
        Range = 6;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade && AbilityRank == 2)
        {
            Owner.StatsChange(AtkChg: 1);
        }

        base.Trigger(sender, e);
    }

    public override void RankUpToTwo()
    {
    }

    public override void UpdateDescription()
    {
        string prefix = AbilityRank < 2 ? "R" : "Gain 1 attack then r";
        Description = prefix + "anged Attack: " + Range;
    }
}
