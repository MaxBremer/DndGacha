using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterCreatureDiesWhileOnboardAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening("AfterCreatureDies", ConditionalTrigger);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening("AfterCreatureDies", ConditionalTrigger);
    }
}
