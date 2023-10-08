using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterCreatureDiesWhileOnboardAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.AfterCreatureDies, ConditionalTrigger, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening(GachaEventType.AfterCreatureDies, ConditionalTrigger, Priority);
    }
}
