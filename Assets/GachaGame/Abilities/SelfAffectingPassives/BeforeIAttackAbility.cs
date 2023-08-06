using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeIAttackAbility : BeforeAttackAbility
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is AttackArgs && sender is Creature c && Owner == c)
        {
            ExternalTrigger(sender, e);
        }
    }
}
