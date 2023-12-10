using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class UnpredictableRageAbility : AfterISurviveDamageAbility
{
    public UnpredictableRageAbility()
    {
        Name = "UnpredictableRage";
        DisplayName = "UNPREDICTABLE RAGE";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        var r = new Random();
        var h = 0;
        var a = 0;
        var s = 0;
        for (int i = 0; i < 4; i++)
        {
            var selection = r.Next(0, 3);
            if(selection == 0)
            {
                h += 1;
            }else if(selection == 1)
            {
                a += 1;
            }
            else
            {
                s += 1;
            }
        }

        Owner.StatsChange(AtkChg: a, HealthChg: h, SpeedChg: s);
    }

    public override void UpdateDescription()
    {
        string suffix = "";

        if(AbilityRank == 1)
        {
            suffix = " This triggers twice.";
        }else if (AbilityRank == 2)
        {
            suffix = " This triggers thrice.";
        }

        Description = "After this character survives damage increase its stats by 4 points distributed randomly between speed, attack, and health." + suffix;
    }
}
