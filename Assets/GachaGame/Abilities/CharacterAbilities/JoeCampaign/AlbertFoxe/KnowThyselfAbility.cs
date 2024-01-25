using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class KnowThyselfAbility : TargetNonselfFriendlyAbility
{
    private int _cooldownReducAmount = 2;

    public KnowThyselfAbility()
    {
        Name = "KnowThyself";
        DisplayName = "Know Thyself";
        MaxCooldown = 0;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.LowerAbilityCooldownsAmount(_cooldownReducAmount);
        }
    }

    public override void UpdateDescription()
    {
        Description = "Select a friendly character. Reduce the current cooldowns of their recharging effects by " + _cooldownReducAmount + ".";
    }

    public override void RankUpToOne()
    {
        _cooldownReducAmount++;
    }

    public override void RankUpToTwo()
    {
        _cooldownReducAmount += 2;
    }
}
