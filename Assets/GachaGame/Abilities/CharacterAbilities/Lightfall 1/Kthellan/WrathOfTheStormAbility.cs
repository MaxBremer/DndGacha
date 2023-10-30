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
        Description = "Ranged Attack 3. This creature gains health equal to its speed.";
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
}
