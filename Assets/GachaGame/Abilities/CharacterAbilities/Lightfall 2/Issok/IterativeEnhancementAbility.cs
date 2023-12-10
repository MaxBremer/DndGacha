using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class IterativeEnhancementAbility : ActiveAbility
{
    private int statPointNum = 1;

    public IterativeEnhancementAbility()
    {
        Name = "IterativeEnhancement";
        DisplayName = "Iterative Enhancement";
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

    public override void RankUpToTwo()
    {
        statPointNum++;
    }

    public override void UpdateDescription()
    {
        Description = "For the rest of this battle, this characters Healing Words ability also gives the target " + statPointNum + " random stat point" + (statPointNum == 1 ? "" : "s") + " (Speed/Health/Attack). This ability stacks with itself.";
    }
}
