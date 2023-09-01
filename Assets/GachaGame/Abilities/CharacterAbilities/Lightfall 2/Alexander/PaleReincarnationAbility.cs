using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PaleReincarnationAbility : ActiveAbility
{
    public PaleReincarnationAbility()
    {
        Name = "PaleReincarnation";
        DisplayName = "Pale Reincarnation";
        Description = "Summon a 1-health copy of a random friendly character that died this game. They gain \"P: At the end of your turn, this gains 1 health\"";
        MaxCooldown = 1;
    }

    // Needs at least 1 graveyard creat and available adjacent space.
    public override bool IsActivateable()
    {
        return base.IsActivateable() && Owner.Controller.Graveyard.Count > 0 && Owner.MyGame.GameGrid.GetUnblockedAdjacents(Owner.MySpace, false).Any();
    }

    public override void Trigger(object sender, EventArgs e)
    {
        var r = new Random();
        var targetC = Owner.Controller.Graveyard[r.Next(0, Owner.Controller.Graveyard.Count)];
        var newC = targetC.CreateCopy();
        newC.MaxHealth = 1;
        newC.Health = 1;
        newC.CreatureTypes.Add("Pale Reincarnation");
        newC.SetController(Owner.Controller);
        var space = Owner.MyGame.GameGrid.GetUnblockedAdjacents(Owner.MySpace, false).First();
        Owner.MyGame.SummonCreature(newC, space);
        newC.GainAbility("DustGraftedBody", true);
    }
}
