using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeAttackAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening("BeforeAttack", ConditionalTrigger);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening("BeforeAttack", ConditionalTrigger);
    }
}
