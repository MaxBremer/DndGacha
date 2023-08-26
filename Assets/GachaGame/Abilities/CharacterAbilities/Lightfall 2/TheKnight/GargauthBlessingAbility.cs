using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GargauthBlessingAbility : TargetFriendlyOrSelfAbility
{
    public GargauthBlessingAbility()
    {
        Name = "GargauthsBlessing";
        DisplayName = "Gargauth's Blessing";
        Description = "Give a friendly creature or this character +0/+8/+4 (speed/health/attack)";
        MaxCooldown = 4;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.StatsChange(AtkChg: 4, HealthChg: 8);
        }
    }
}
