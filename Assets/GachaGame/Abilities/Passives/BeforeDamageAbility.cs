using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeDamageAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening("BeforeDamage", ConditionalTrigger);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening("BeforeDamage", ConditionalTrigger);
    }
}
