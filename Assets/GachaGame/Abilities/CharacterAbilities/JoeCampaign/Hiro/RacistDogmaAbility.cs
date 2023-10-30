using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class RacistDogmaAbility : AbilityCooldownLowerPassive
{
    public RacistDogmaAbility()
    {
        Name = "RacistDogma";
        DisplayName = "Racist Dogma";
        Description = "While a character is within Range 1 of this one, it does not recharge its abilities.";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(e is CooldownEventArgs cooldownArgs && cooldownArgs.AbilityOwner != Owner && GachaGrid.IsInRange(Owner, cooldownArgs.AbilityOwner, 1))
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is CooldownEventArgs cooldownArgs)
        {
            cooldownArgs.CooldownAmount = 0;
        }
    }
}
