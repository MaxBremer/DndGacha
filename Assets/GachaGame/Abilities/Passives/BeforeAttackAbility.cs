using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeAttackAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.BeforeAttack, ConditionalTrigger, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening(GachaEventType.BeforeAttack, ConditionalTrigger, Priority);
    }
}
