using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class TheTigerAbility : TouchRangeEnemyCreatureAbility
{
    private int atkGainAmt = 2;
    private int numAtks = 3;

    public TheTigerAbility()
    {
        Name = "TheTiger";
        DisplayName = "The Tiger";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            Owner.StatsChange(AtkChg: atkGainAmt);
            for (int i = 0; i < numAtks; i++)
            {
                if (Owner.IsOnBoard && creatChoice.TargetCreature.IsOnBoard)
                {
                    Owner.AttackTarget(creatChoice.TargetCreature);
                }
            }
        }
    }

    public override void UpdateDescription()
    {
        Description = "Choose an adjacent enemy. Gain " + atkGainAmt + " attack, then attack the target " + numAtks + " times.";
    }

    public override void RankUpToOne()
    {
        atkGainAmt++;
        MaxCooldown--;
    }

    public override void RankUpToTwo()
    {
        atkGainAmt++;
        numAtks++;
    }
}
