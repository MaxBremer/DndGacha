using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HelltechArmorAbility : BeforeMyStatsChangeAbility
{
    public HelltechArmorAbility()
    {
        Name = "HelltechArmor";
        DisplayName = "Helltech Armor";
        Description = "Whenever this creature gains non-aura stats, it gains twice as much.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        base.Trigger(sender, e);
        if(e is StatChangeArgs chgArgs && chgArgs.AreStatsPermanent)
        {
            chgArgs.AttackChange = chgArgs.AttackChange > 0 ? chgArgs.AttackChange * 2 : chgArgs.AttackChange;
            chgArgs.HealthChange = chgArgs.HealthChange > 0 ? chgArgs.HealthChange * 2 : chgArgs.HealthChange;
            chgArgs.SpeedChange = chgArgs.SpeedChange > 0 ? chgArgs.SpeedChange * 2 : chgArgs.SpeedChange;
            chgArgs.InitChange = chgArgs.InitChange > 0 ? chgArgs.InitChange * 2 : chgArgs.InitChange;
        }
    }
}
