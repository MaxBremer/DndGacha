using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class HarshLeadershipAbility : RangedTargetFriendlyAbility
{
    private int _dmgAmount = 6;

    public HarshLeadershipAbility()
    {
        Name = "HarshLeadership";
        DisplayName = "Harsh Leadership";
        MaxCooldown = 1;
        Range = 3;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.TakeDamage(_dmgAmount, Owner);
            creatChoice.TargetCreature.CanAct = true;
        }
    }

    public override void UpdateDescription()
    {
        Description = "Choose a friendly character within Range " + Range + ". They may act again. Deal " + _dmgAmount + " damage to them.";
    }

    public override void RankUpToOne()
    {
        _dmgAmount -= 2;
    }

    public override void RankUpToTwo()
    {
        Range++;
        MaxCooldown = Math.Max(MaxCooldown - 1, 0);
    }
}
