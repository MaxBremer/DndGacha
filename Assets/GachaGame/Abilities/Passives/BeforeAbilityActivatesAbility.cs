using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BeforeAbilityActivatesAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.BeforeActiveAbilityActivates, ConditionalTrigger, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening(GachaEventType.BeforeActiveAbilityActivates, ConditionalTrigger, Priority);
    }
}
