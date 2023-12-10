using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class DustGraftedBodyAbility : TurnEndPassive
{
    private int HealthAmount;

    public DustGraftedBodyAbility(int hpAmt = 1)
    {
        Name = "DustGraftedBody";
        DisplayName = "Dust-grafted Body";
        HealthAmount = hpAmt;
        Description = "At the end of each turn, this gains " + HealthAmount + " health.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.StatsChange(HealthChg: HealthAmount);
    }
}
