using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FriendlyCreatureSummonedPassive : CreatureSummonedAbility
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is CreatureSummonArgs cArgs && cArgs.BeingSummoned.Controller == Owner.Controller)
        {
            ExternalTrigger(sender, e);
        }
    }
}
