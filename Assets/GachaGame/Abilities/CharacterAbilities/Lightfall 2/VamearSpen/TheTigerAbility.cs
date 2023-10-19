using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TheTigerAbility : TouchRangeEnemyCreatureAbility
{
    public TheTigerAbility()
    {
        Name = "TheTiger";
        DisplayName = "The Tiger";
        Description = "Gain 2 attack, then attack the target 3 times.";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            Owner.StatsChange(AtkChg: 2);
            for (int i = 0; i < 3; i++)
            {
                if (Owner.IsOnBoard && creatChoice.TargetCreature.IsOnBoard)
                {
                    Owner.AttackTarget(creatChoice.TargetCreature);
                }
            }
        }
    }
}
