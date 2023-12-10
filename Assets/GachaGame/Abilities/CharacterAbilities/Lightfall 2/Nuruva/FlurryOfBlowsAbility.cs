using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class FlurryOfBlowsAbility : TouchRangeEnemyCreatureAbility
{
    public FlurryOfBlowsAbility()
    {
        Name = "FlurryOfBlows";
        DisplayName = "Flurry of Blows";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").First() is CreatureTargetChoice creatChoice)
        {
            var targetCreat = creatChoice.TargetCreature;
            Owner.AttackTarget(targetCreat);
            if(AbilityRank >= 1)
            {
                Owner.AttackTarget(targetCreat);
            }
            Owner.GainTag(CreatureTag.IMMUNE);
            Owner.AttackTarget(targetCreat);
            Owner.LoseTag(CreatureTag.IMMUNE);
        }
    }

    public override void RankUpToOne()
    {
    }

    public override void UpdateDescription()
    {
        Description = (AbilityRank < 1 ? "Attack, then Attack the same target again." : "Attack the same target thrice.") + " This character is Immune during the " + (AbilityRank < 1 ? "second" : "third") + " attack.";
    }
}
