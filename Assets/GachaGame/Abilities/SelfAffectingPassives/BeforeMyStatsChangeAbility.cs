using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeMyStatsChangeAbility : BeforeStatChangeAbility
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is StatChangeArgs && sender is Creature c && Owner == c)
        {
            ExternalTrigger(sender, e);
        }
    }
}
