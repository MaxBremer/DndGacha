using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDiesWhileOnboardAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening("CreatureDies", ConditionalTrigger);
    }

    public override void RemoveGraveyardTriggers()
    {
        base.RemoveGraveyardTriggers();
        EventManager.StopListening("CreatureDies", ConditionalTrigger);
    }
}
