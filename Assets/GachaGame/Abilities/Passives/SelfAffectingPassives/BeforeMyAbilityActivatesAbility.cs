using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BeforeMyAbilityActivatesAbility : BeforeAbilityActivatesAbility
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(e is AbilityActivateArgs abilArgs && abilArgs.AbilityActivating.Owner == Owner)
        {
            ExternalTrigger(sender, e);
        }
    }
}
