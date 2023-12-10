using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class BlazingReincarnationAbility : AfterCreatureDiesWhileGraveyardAbility
{
    public BlazingReincarnationAbility()
    {
        Name = "BlazingReincarnation";
        DisplayName = "Blazing Reincarnation";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        //TODO: Don't need to check if not blocked?? If use nearest available space?
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
            if (AbilityRank >= 1)
            {
                Owner.StatsChange(SpeedChg: 1);
            }
            Owner.MyGame.SummonCreature(Owner, dieArgs.WhereItDied);
            if (AbilityRank < 2)
            {
                Owner.RemoveAbility(this);
            }
        }
    }

    public override void UpdateDescription()
    {
        Description = "If this character dies while Smolder is on the battlefield, resummon it in the same position it was in at full health" + (AbilityRank < 1 ? "" : " and +1 Speed") + (AbilityRank < 2 ? " without this ability." : ".");
    }

    public override void RankUpToOne()
    {
    }

    public override void RankUpToTwo()
    {
    }
}
