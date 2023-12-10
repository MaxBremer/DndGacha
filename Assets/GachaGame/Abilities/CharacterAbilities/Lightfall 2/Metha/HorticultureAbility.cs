using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class HorticultureAbility : ActiveAbility
{
    private int StatAmount = 1;

    public HorticultureAbility()
    {
        Name = "Horticulture";
        DisplayName = "Horticulture";
        MaxCooldown = 1;
    }

    // Can only trigger if you have plants on the battlefield.
    public override bool IsActivateable()
    {
        return base.IsActivateable() && Owner.Controller.OnBoardCreatures.Where(x => x.CreatureTypes.Contains("Plant")).Any();
    }

    public override void Trigger(object sender, EventArgs e)
    {
        foreach (var creat in Owner.Controller.OnBoardCreatures.Where(x => x.CreatureTypes.Contains("Plant")))
        {
            creat.StatsChange(AtkChg: StatAmount, HealthChg: StatAmount);
        }
    }

    public override void RankUpToTwo()
    {
        StatAmount++;
    }

    public override void UpdateDescription()
    {
        Description = "Give your plants +0/+" + StatAmount + "/+" + StatAmount + ".";
    }
}
