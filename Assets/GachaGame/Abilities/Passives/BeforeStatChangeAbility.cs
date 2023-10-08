using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeStatChangeAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.BeforeCreatureStatsChange, ConditionalTrigger, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening(GachaEventType.BeforeCreatureStatsChange, ConditionalTrigger, Priority);
    }
}
