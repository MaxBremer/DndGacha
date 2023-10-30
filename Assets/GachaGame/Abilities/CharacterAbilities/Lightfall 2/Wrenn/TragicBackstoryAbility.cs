using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class TragicBackstoryAbility : MyTurnEndPassive
{
    public TragicBackstoryAbility()
    {
        Name = "TragicBackstory";
        DisplayName = "Tragic Backstory";
        Description = "At the end of your turn, this character gains 2 attack.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.StatsChange(AtkChg: 2);
    }
}
