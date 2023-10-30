using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class KnowThyEnemyAbility : TargetSingleEnemyAbility
{
    public KnowThyEnemyAbility()
    {
        Name = "KnowThyEnemy";
        DisplayName = "Know Thy Enemy";
        Description = "Select an enemy character. Reduce their attack by 3.";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.StatsChange(AtkChg: -3);
        }
    }
}
