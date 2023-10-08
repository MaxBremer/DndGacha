using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BeforeCreatureDiesWhileOnboardAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.BeforeCreatureDies, ConditionalTrigger, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveGraveyardTriggers();
        EventManager.StopListening(GachaEventType.BeforeCreatureDies, ConditionalTrigger, Priority);
    }
}
