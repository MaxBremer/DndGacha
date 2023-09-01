using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AfterISurviveDamageAbility : AfterDamageAbility
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is TakingDamageArgs && sender is Creature c && Owner == c && Owner.IsOnBoard)
        {
            ExternalTrigger(sender, e);
        }
    }
}
