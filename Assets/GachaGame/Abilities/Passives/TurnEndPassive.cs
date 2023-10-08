using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndPassive : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.EndOfTurn, ConditionalTrigger, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening(GachaEventType.EndOfTurn, ConditionalTrigger, Priority);
    }
}
