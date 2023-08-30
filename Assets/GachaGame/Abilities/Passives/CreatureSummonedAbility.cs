using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreatureSummonedAbility : PassiveAbility
{
    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening("CreatureSummoned", ConditionalTrigger);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening("CreatureSummoned", ConditionalTrigger);
    }
}
