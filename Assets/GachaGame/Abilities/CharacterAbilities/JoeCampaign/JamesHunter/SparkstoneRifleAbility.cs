using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SparkstoneRifleAbility : OrthogonalTargetEnemyAbility
{
    public SparkstoneRifleAbility()
    {
        Name = "SparkstoneRifle";
        DisplayName = "Sparkstone Rifle";
        Description = "Make a ranged attack of any distance in an orthogonal line.";
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
}
