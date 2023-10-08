using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCalledAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.CreatureCalled, ConditionalTrigger, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening(GachaEventType.CreatureCalled, ConditionalTrigger, Priority);
    }
}
