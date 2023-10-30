using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class IterativeEnhancementAbility : ActiveAbility
{
    public IterativeEnhancementAbility()
    {
        Name = "IterativeEnhancement";
        DisplayName = "Iterative Enhancement";
        Description = "For the rest of this battle, Healing Words also gives the target 1 random stat point (Speed/Health/Attack). This ability stacks with itself.";
        MaxCooldown = 1;
    }

    public override bool IsActivateable()
    {
        return base.IsActivateable() && Owner.HasAbility("HealingWords");
    }

    public override void Trigger(object sender, EventArgs e)
    {
        var healAbil = Owner.Abilities.Where(x => x is HealingWordsAbility).First() as HealingWordsAbility;
        healAbil.RandomStatPoints += 1;
    }
}
