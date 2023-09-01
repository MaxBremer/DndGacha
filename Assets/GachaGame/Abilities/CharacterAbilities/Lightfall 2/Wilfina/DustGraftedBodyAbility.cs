using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DustGraftedBodyAbility : TurnEndPassive
{
    public DustGraftedBodyAbility()
    {
        Name = "DustGraftedBody";
        DisplayName = "Dust-grafted Body";
        Description = "At the end of each turn, this gains 1 health.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.StatsChange(HealthChg: 1);
    }
}
