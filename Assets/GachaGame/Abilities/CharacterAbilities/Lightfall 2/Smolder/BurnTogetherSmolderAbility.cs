using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BurnTogetherSmolderAbility : CreatureAffectingAuraWhileOnboardAbility
{
    private const int STAT_CHANGE_AMOUNT = 2;

    public BurnTogetherSmolderAbility()
    {
        Name = "BurnTogetherSmolder";
        DisplayName = "Burn Together";
        Description = "If Buzz is on the battlefield, her stats have +" + STAT_CHANGE_AMOUNT;
    }

    public override void ApplyEffectToCreature(Creature c)
    {
        c.StatsChange(STAT_CHANGE_AMOUNT, STAT_CHANGE_AMOUNT, STAT_CHANGE_AMOUNT, STAT_CHANGE_AMOUNT);
    }

    public override void RemoveEffectFromCreature(Creature c)
    {
        var amt = -1 * STAT_CHANGE_AMOUNT;
        c.StatsChange(amt, amt, amt, amt);
    }

    public override bool ShouldCreatureBeEffected(Creature c)
    {
        return c.DisplayName == "Buzz";
    }
}
