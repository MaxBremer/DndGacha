using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class AgmalsPromiseAbility : AfterMyDeathPassive
{
    public Creature PromiseGiver;

    public AgmalsPromiseAbility()
    {
        Name = "AgmalsPromise";
        DisplayName = "Agmal's Promise";
        Description = "When this creature would die, instead heal it for 5, then replace this with Agmal's Pact.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is CreatureDiesArgs dieArgs && Owner.State == CreatureState.GRAVEYARD && (!dieArgs.WhereItDied.isBlocked))
        {

            Owner.Heal(5);
            Owner.MyGame.SummonCreature(Owner, dieArgs.WhereItDied);
            var pact = new AgmalsPactAbility();
            pact.PactGiver = PromiseGiver;
            Owner.GainAbility(pact, true);
            Owner.RemoveAbility(this);
        }
    }
}