using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BeforeOtherFriendlyTakesDamageAbility : BeforeDamageAbility
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is TakingDamageArgs && sender is Creature c && Owner != c && Owner.Controller == c.Controller)
        {
            ExternalTrigger(sender, e);
        }
    }
}
