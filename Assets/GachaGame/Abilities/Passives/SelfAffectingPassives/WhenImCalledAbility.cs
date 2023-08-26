using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenImCalledAbility : CreatureCalledAbility
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is CreatureSummonArgs sumArgs && sumArgs.BeingSummoned == Owner)
        {
            ExternalTrigger(sender, e);
        }
    }
}
