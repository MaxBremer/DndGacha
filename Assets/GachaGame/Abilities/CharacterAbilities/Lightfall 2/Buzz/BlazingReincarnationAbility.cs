using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BlazingReincarnationAbility : AfterCreatureDiesWhileOnboardAbility
{
    public BlazingReincarnationAbility()
    {
        Name = "BlazingReincarnation";
        DisplayName = "Blazing Reincarnation";
        Description = "If this character dies while Smolder is on the battlefield, resummon it in the same position it was in at full health without this ability.";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is CreatureDiesArgs dieArgs && dieArgs.CreatureDied == Owner && Owner.InGraveyard && Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && x.DisplayName == "Smolder").Any() && dieArgs.WhereItDied.isBlocked == false)
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is CreatureDiesArgs dieArgs)
        {
            Owner.Health = Owner.MaxHealth;
            Owner.MyGame.SummonCreature(Owner, dieArgs.WhereItDied);
            Owner.RemoveAbility(this);
        }
    }
}
