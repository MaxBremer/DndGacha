using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeITakeDamagePassiveAbility : BeforeDamageAbility
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is TakingDamageArgs && sender is Creature c && Owner == c)
        {
            ExternalTrigger(sender, e);
        }
    }
}
