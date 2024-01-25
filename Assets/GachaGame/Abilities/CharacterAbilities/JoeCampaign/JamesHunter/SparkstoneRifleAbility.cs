using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class SparkstoneRifleAbility : OrthogonalTargetEnemyAbility
{
    private int _dmgBonus = 0;

    public SparkstoneRifleAbility()
    {
        Name = "SparkstoneRifle";
        DisplayName = "Sparkstone Rifle";
        MaxCooldown = 0;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Options").FirstOrDefault() is ConditionalOptionSelectChoice condOptChoice && condOptChoice.ChoiceMade)
        {
            var trueTarget = GetTargetForDir(condOptChoice.ChosenOption);

            Owner.AttackTarget(trueTarget, true);
        }
    }

    public override void UpdateDescription()
    {
        string dmgBonStr = _dmgBonus == 0 ? "" : " with +" + _dmgBonus + " damage";

        Description = "Make a ranged attack" + dmgBonStr + " of any distance in an orthogonal line.";
    }

    public override void RankUpToOne()
    {
        _dmgBonus++;
    }

    public override void RankUpToTwo()
    {
        _dmgBonus += 2;
    }
}
