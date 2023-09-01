using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AfterITakeDamageAbility : AfterDamageAbility
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is TakingDamageArgs && sender is Creature c && Owner == c)
        {
            ExternalTrigger(sender, e);
        }
    }
}
