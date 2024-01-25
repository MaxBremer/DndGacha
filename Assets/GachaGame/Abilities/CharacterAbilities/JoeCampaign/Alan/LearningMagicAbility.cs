using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class LearningMagicAbility : MyTurnEndPassive
{
    private int _atkAmt = 1;
    private int _healthAmt = 0;

    public LearningMagicAbility()
    {
        Name = "LearningMagic";
        DisplayName = "Learning Magic";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.StatsChange(AtkChg: _atkAmt, HealthChg: _healthAmt);
    }

    public override void UpdateDescription()
    {
        string suffix = (_healthAmt > 0 ? " and " + _healthAmt + " health." : ".");
        Description = "At the end of your turn this character gains " + _atkAmt + " attack" + suffix;
    }

    public override void RankUpToOne()
    {
        _healthAmt++;
    }

    public override void RankUpToTwo()
    {
        _atkAmt++;
    }
}
