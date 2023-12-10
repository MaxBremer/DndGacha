using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class GargauthBlessingAbility : TargetFriendlyOrSelfAbility
{
    private int healthAmt = 8;
    private int atkAmt = 4;

    public GargauthBlessingAbility()
    {
        Name = "GargauthsBlessing";
        DisplayName = "Gargauth's Blessing";
        MaxCooldown = 4;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.StatsChange(AtkChg: 4, HealthChg: 8);
        }
    }

    public override void UpdateDescription()
    {
        Description = "Give a friendly creature or this character +0/+" + healthAmt + "/+" + atkAmt + " (speed/health/attack)";
    }

    public override void RankUpToOne()
    {
        healthAmt += 2;
        atkAmt++;
    }
}
