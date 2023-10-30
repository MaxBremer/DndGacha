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
        Description = "Attack, then Attack the same target again. This character is Immune during the second attack.";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").First() is CreatureTargetChoice creatChoice)
        {
            var targetCreat = creatChoice.TargetCreature;
            Owner.AttackTarget(targetCreat);
            Owner.GainTag(CreatureTag.IMMUNE);
            Owner.AttackTarget(targetCreat);
            Owner.LoseTag(CreatureTag.IMMUNE);
        }
    }
}
