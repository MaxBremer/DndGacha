using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ArmCannonAbility : RangedAttackEnemiesAbility
{
    private int atkAmt = 1;

    public ArmCannonAbility()
    {
        Name = "ArmCannon";
        DisplayName = "Arm Cannon";
        Range = 1;
        MaxCooldown = 2;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.StatsChange(AtkChg: atkAmt);
        base.Trigger(sender, e);
        Owner.StatsChange(AtkChg: -1 * atkAmt);
    }

    public override void UpdateDescription()
    {
        Description = "Gain " + atkAmt + " attack, then Ranged Attack: " + Range + ", then lose " + atkAmt + " attack.";
    }

    public override void RankUpToTwo()
    {
        atkAmt++;
    }
}
