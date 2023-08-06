using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCannonAbility : RangedAttackEnemiesAbility
{
    public ArmCannonAbility()
    {
        Name = "ArmCannon";
        DisplayName = "Arm Cannon";
        Description = "Gain 1 attack, then Ranged Attack: 1, then lose 1 attack.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.StatsChange(AtkChg: 1);
        base.Trigger(sender, e);
        Owner.StatsChange(AtkChg: -1);
    }
}
