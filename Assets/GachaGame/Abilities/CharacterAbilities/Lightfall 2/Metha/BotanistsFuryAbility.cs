using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class BotanistsFuryAbility : AfterCreatureDiesWhileOnboardAbility
{
    public BotanistsFuryAbility()
    {
        Name = "BotanistsFury";
        DisplayName = "Botanist's Fury";
        Description = "When a plant dies, give this +1 attack.";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(e is CreatureDiesArgs dieArgs && dieArgs.CreatureDied.CreatureTypes.Contains("Plant"))
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.StatsChange(AtkChg: 1);
    }
}
