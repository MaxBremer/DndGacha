using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AfterCreatureDiesWhileGraveyardAbility : PassiveAbility
{
    public override void AddGraveyardTriggers()
    {
        base.AddGraveyardTriggers();
        EventManager.StartListening("AfterCreatureDies", ConditionalTrigger);
    }

    public override void RemoveGraveyardTriggers()
    {
        base.RemoveGraveyardTriggers();
        EventManager.StopListening("AfterCreatureDies", ConditionalTrigger);
    }
}
