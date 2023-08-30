using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FriendlyCreatureDiesPassive : AfterCreatureDiesWhileOnboardAbility
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is CreatureDiesArgs dieArgs && dieArgs.CreatureDied.Controller == Owner.Controller)
        {
            ExternalTrigger(sender, e);
        }
    }
}
