using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FriendlyNonselfCreatureSummonedPassive : CreatureSummonedAbility
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is CreatureSummonArgs cArgs && cArgs.BeingSummoned.Controller == Owner.Controller && cArgs.BeingSummoned != Owner)
        {
            ExternalTrigger(sender, e);
        }
    }
}
