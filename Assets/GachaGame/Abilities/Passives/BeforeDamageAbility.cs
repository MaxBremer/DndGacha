using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeDamageAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.BeforeDamage, ConditionalTrigger, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening(GachaEventType.BeforeDamage, ConditionalTrigger, Priority);
    }
}
