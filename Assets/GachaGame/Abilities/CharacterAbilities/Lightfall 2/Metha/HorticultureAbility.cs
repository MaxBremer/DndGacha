using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HorticultureAbility : ActiveAbility
{
    public HorticultureAbility()
    {
        Name = "Horticulture";
        DisplayName = "Horticulture";
        Description = "Give your plants +0/+1/+1.";
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
            creat.StatsChange(AtkChg: 1, HealthChg: 1);
        }
    }
}
