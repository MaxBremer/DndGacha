using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeStatChangeAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening("BeforeCreatureStatsChange", ConditionalTrigger);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening("BeforeCreatureStatsChange", ConditionalTrigger);
    }
}
