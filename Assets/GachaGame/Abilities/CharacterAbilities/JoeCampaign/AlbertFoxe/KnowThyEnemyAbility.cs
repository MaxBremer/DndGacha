using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class KnowThyEnemyAbility : TargetSingleEnemyAbility
{
    private int _atkReducAmount = 3;

    public KnowThyEnemyAbility()
    {
        Name = "KnowThyEnemy";
        DisplayName = "Know Thy Enemy";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.StatsChange(AtkChg: -1 * _atkReducAmount);
        }
    }

    public override void UpdateDescription()
    {
        Description = "Select an enemy character. Reduce their attack by " + _atkReducAmount + ".";
    }

    public override void RankUpToOne()
    {
        _atkReducAmount++;
    }
}
