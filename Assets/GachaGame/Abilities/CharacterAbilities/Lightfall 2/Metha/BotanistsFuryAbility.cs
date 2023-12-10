using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class BotanistsFuryAbility : AfterCreatureDiesWhileOnboardAbility
{
    private int AtkAmount = 1;

    public BotanistsFuryAbility()
    {
        Name = "BotanistsFury";
        DisplayName = "Botanist's Fury";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(e is CreatureDiesArgs dieArgs && (dieArgs.CreatureDied.CreatureTypes.Contains("Plant") || AbilityRank == 2))
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.StatsChange(AtkChg: AtkAmount);
    }

    public override void RankUpToOne()
    {
        AtkAmount++;
    }

    public override void RankUpToTwo()
    {
    }

    public override void UpdateDescription()
    {
        Description = "When a " + (AbilityRank < 2 ? "plant" : "creature") + " dies, give this +" + AtkAmount + " attack.";
    }
}
