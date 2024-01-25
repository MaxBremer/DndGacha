using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class WrathOfTheStormAbility : RangedTargetEnemyAbility
{
    public WrathOfTheStormAbility()
    {
        Name = "WrathOfTheStorm";
        DisplayName = "Wrath of the Storm";
        MaxCooldown = 2;
        Range = 3;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").First() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            Owner.AttackTarget(creatChoice.TargetCreature, true);
            Owner.StatsChange(HealthChg: Owner.Speed);
        }
    }

    public override void UpdateDescription()
    {
        Description = "Ranged Attack " + Range + ". This creature gains health equal to its speed.";
    }
}
