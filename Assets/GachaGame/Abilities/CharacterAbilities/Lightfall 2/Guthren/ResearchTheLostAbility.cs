using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ResearchTheLostAbility : RangedUnblockedPointTargetAbility
{
    public ResearchTheLostAbility()
    {
        Name = "ResearchTheLost";
        DisplayName = "Research the Lost";
        Description = "Summon a copy of a random creature that's died this game in range 1. Set its stats to 1. This gains a random one of its abilities if it has any. Otherwise, this gains 2 health. Increase the cooldown of this ability by 1.";
        MaxCooldown = 1;
        Range = 1;
    }

    // Only activateable if there is at least one creature in controllers graveyard.
    public override bool IsActivateable()
    {
        return base.IsActivateable() && Owner.Controller.Graveyard.Count > 0;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            var r = new Random();
            var targetCreat = Owner.Controller.Graveyard[r.Next(0, Owner.Controller.Graveyard.Count)];
            var newCreat = targetCreat.CreateCopy();
            newCreat.StatsSet(1, 1, 1, 1);
            newCreat.SetController(Owner.Controller);
            Owner.MyGame.SummonCreature(newCreat, pointChoice.TargetSpace);

            if(newCreat.Abilities.Count > 0)
            {
                Owner.GainAbility(newCreat.Abilities[r.Next(0, newCreat.Abilities.Count)].CreateCopy());
            }
            else
            {
                Owner.StatsChange(HealthChg: 2);
            }

            MaxCooldown += 1;
        }
    }
}
