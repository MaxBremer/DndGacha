using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WhenImSummonedAbility : CreatureSummonedAbility
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is CreatureSummonArgs sumArgs && sumArgs.BeingSummoned == Owner)
        {
            ExternalTrigger(sender, e);
        }
    }
}
