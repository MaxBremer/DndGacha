using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class KnowThyselfAbility : TargetNonselfFriendlyAbility
{
    public KnowThyselfAbility()
    {
        Name = "KnowThyself";
        DisplayName = "Know Thyself";
        Description = "Select a friendly character. Reduce the current cooldowns of their recharging effects by 3.";
        MaxCooldown = 0;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.LowerAbilityCooldownsAmount(3);
        }
    }
}
