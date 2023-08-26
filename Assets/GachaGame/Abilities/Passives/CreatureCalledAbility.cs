using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCalledAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening("CreatureCalled", ConditionalTrigger);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening("CreatureCalled", ConditionalTrigger);
    }
}
