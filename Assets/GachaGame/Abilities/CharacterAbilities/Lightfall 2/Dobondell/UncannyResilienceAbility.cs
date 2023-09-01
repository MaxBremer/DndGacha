using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UncannyResilienceAbility : AfterMyDeathPassive
{
    public UncannyResilienceAbility()
    {
        Name = "UncannyResillience";
        DisplayName = "UNCANNY RESILLIENCE";
        Description = "When this character takes damage that would kill it, instead set its health equal to its attack and remove this ability.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is CreatureDiesArgs dieArgs)
        {
            Owner.StatsSet(HealthSet: Owner.Attack);
            Owner.MyGame.SummonCreature(Owner, dieArgs.WhereItDied);
            Owner.RemoveAbility(this);
        }
    }
}
