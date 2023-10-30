using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class HealingWordsAbility : TouchRangeFriendlyOrSelfAbility
{
    private const int HEALING_AMOUNT = 4;

    public int RandomStatPoints = 0;

    public HealingWordsAbility()
    {
        Name = "HealingWords";
        DisplayName = "Healing Words";
        Description = "Restore " + HEALING_AMOUNT + " health to a character in range 3, or to this character.";
        MaxCooldown = 0;
        Range = 3;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.Heal(HEALING_AMOUNT);
            if(RandomStatPoints > 0)
            {
                var r = new Random();
                int atkAmt = 0, healthAmt = 0, spdAmt = 0;
                for (int i = 0; i < RandomStatPoints; i++)
                {
                    int rand = r.Next(1, 4);
                    if (rand == 1)
                    {
                        atkAmt += 1;
                    }
                    else if (rand == 2)
                    {
                        healthAmt += 1;
                    }
                    else
                    {
                        spdAmt += 1;
                    }
                }

                creatChoice.TargetCreature.StatsChange(AtkChg: atkAmt, HealthChg: healthAmt, SpeedChg: spdAmt);
            }
        }
    }
}
