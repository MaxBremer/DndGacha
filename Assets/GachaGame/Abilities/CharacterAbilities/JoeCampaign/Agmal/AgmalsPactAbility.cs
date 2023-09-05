using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AgmalsPactAbility : MyTurnStartPassive
{
    public Creature PactGiver;

    public AgmalsPactAbility()
    {
        Name = "AgmalsPact";
        DisplayName = "Agmal's Pact";
        Description = "At the start of your turn, deal 1 damage to all friendly characters, and give the character that granted this character *Agmal's Promise* +0/4/1 if it is on the board.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && x.Controller == Owner.Controller).ToList().ForEach(x => x.TakeDamage(1, Owner));
        if(PactGiver != null && PactGiver.IsOnBoard)
        {
            PactGiver.StatsChange(AtkChg: 1, HealthChg: 4 );
        }
    }
}
