using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class AspectOfDustAbility : AfterMyDeathPassive
{
    private int _healAmount = 1;
    
    public AspectOfDustAbility()
    {
        Name = "AspectOfDust";
        DisplayName = "Aspect of Dust";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is CreatureDiesArgs dieArgs)
        {
            Owner.Heal(_healAmount);
            var chgMult = AbilityRank < 2 ? 1 : 2;
            Owner.StatsChange(AtkChg: Owner.Attack * chgMult, SpeedChg: Owner.Speed * chgMult);
            Owner.MyGame.SummonCreature(Owner, dieArgs.WhereItDied);
            Owner.RemoveAbility(this);
        }
    }

    public override void UpdateDescription()
    {
        Description = "When this creture would be killed, instead it is healed for " + _healAmount + " and resummoned, loses Aspect of Dust, and its attack and speed are " + (AbilityRank < 2 ? "doubled." : "tripled.");
    }

    public override void RankUpToOne()
    {
        _healAmount += 4;
    }

    public override void RankUpToTwo()
    {
    }
}
