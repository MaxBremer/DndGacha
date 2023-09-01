using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LearningMagicAbility : MyTurnEndPassive
{
    public LearningMagicAbility()
    {
        Name = "LearningMagic";
        DisplayName = "Learning Magic";
        Description = "At the end of your turn this character gains 1 attack.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.StatsChange(AtkChg: 1);
    }
}
