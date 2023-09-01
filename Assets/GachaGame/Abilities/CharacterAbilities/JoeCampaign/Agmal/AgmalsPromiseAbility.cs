using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AgmalsPromiseAbility : AfterMyDeathPassive
{
    public Creature PromiseGiver;

    public AgmalsPromiseAbility()
    {
        Name = "AgmalsPromise";
        DisplayName = "Agmal's Promise";
        Description = "When this creature would die, instead restore it to 5 health, then replace this with Agmal's Pact.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is CreatureDiesArgs dieArgs && Owner.State == CreatureState.GRAVEYARD && (!dieArgs.WhereItDied.isBlocked))
        {
            Owner.StatsSet(HealthSet: 5);
            Owner.MyGame.SummonCreature(Owner, dieArgs.WhereItDied);
            Owner.RemoveAbility(this);
            var pact = new AgmalsPactAbility();
            pact.PactGiver = PromiseGiver;
            Owner.GainAbility(pact);
        }
    }
}