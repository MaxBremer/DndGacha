using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HarshLeadershipAbility : RangedTargetFriendlyAbility
{
    public HarshLeadershipAbility()
    {
        Name = "HarshLeadership";
        DisplayName = "Harsh Leadership";
        Description = "Choose a friendly character within Range 3. They may act again. Deal 6 damage to them.";
        MaxCooldown = 1;
        Range = 3;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.TakeDamage(6, Owner);
            creatChoice.TargetCreature.CanAct = true;
        }
    }
}
