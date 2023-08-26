using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndPassive : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening("EndOfTurn", ConditionalTrigger);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening("EndOfTurn", ConditionalTrigger);
    }
}
