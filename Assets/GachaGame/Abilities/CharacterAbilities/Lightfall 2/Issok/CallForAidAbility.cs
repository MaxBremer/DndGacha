using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CallForAidAbility : TargetSingleFriendlyReserveCreatureAbility
{
    public CallForAidAbility()
    {
        Name = "CallForAid";
        DisplayName = "Call for Aid";
        Description = "Reduce the Initiative of a character in reserve by 1.";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.StatsChange(InitChg: -1);
        }
    }
}
