using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HelltechArmorAbility : BeforeMyStatsChangeAbility
{
    private int mult = 2;

    public HelltechArmorAbility()
    {
        Name = "HelltechArmor";
        DisplayName = "Helltech Armor";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is StatChangeArgs chgArgs && chgArgs.AreStatsPermanent)
        {
            chgArgs.AttackChange = chgArgs.AttackChange > 0 ? chgArgs.AttackChange * mult : chgArgs.AttackChange;
            chgArgs.HealthChange = chgArgs.HealthChange > 0 ? chgArgs.HealthChange * mult : chgArgs.HealthChange;
            if(AbilityRank >= 1)
            {
                chgArgs.SpeedChange = chgArgs.SpeedChange > 0 ? chgArgs.SpeedChange * mult : chgArgs.SpeedChange;
                chgArgs.InitChange = chgArgs.InitChange > 0 ? chgArgs.InitChange * mult : chgArgs.InitChange;
            }
        }
    }

    public override void UpdateDescription()
    {
        Description = "Whenever this creature gains non-aura " + (AbilityRank < 1 ? "attack or health" : "stats") + ", it gains " + (mult == 2 ? "twice" : "thrice") + " as much.";
    }

    public override void RankUpToOne()
    {
    }

    public override void RankUpToTwo()
    {
        mult++;
    }
}
