using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AspectOfDustAbility : AfterMyDeathPassive
{
    public AspectOfDustAbility()
    {
        Name = "AspectOfDust";
        DisplayName = "Aspect of Dust";
        Description = "When this creture would be killed, instead it is resummoned at one health, loses Aspect of Dust, and its attack and speed are doubled.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is CreatureDiesArgs dieArgs)
        {
            Owner.StatsSet(HealthSet: 1);
            Owner.StatsChange(AtkChg: Owner.Attack, SpeedChg: Owner.Speed);
            Owner.MyGame.SummonCreature(Owner, dieArgs.WhereItDied);
            Owner.RemoveAbility(this);
        }
    }
}
