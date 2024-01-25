using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class RacistDogmaAbility : AbilityCooldownLowerPassive
{
    private int _dogmaRange = 1;

    public RacistDogmaAbility()
    {
        Name = "RacistDogma";
        DisplayName = "Racist Dogma";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(e is CooldownEventArgs cooldownArgs && cooldownArgs.AbilityOwner != Owner && GachaGrid.IsInRange(Owner, cooldownArgs.AbilityOwner, _dogmaRange) && (AbilityRank < 2 || cooldownArgs.AbilityOwner.Controller != Owner.Controller))
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

    public override void UpdateDescription()
    {
        Description = "While a" + (AbilityRank < 2 ? " " : "n enemy ") + "character is within Range " + _dogmaRange + " of this one, it does not recharge its abilities.";
    }

    public override void RankUpToOne()
    {
        _dogmaRange++;
    }

    public override void RankUpToTwo()
    {
    }
}
