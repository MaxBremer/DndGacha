using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class CallForAidAbility : TargetSingleFriendlyReserveCreatureAbility
{
    private int ReductionAmount = -1;

    public CallForAidAbility()
    {
        Name = "CallForAid";
        DisplayName = "Call for Aid";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.StatsChange(InitChg: ReductionAmount);
        }
    }

    public override void RankUpToOne()
    {
        ReductionAmount--;
    }

    public override void UpdateDescription()
    {
        Description = "Reduce the Initiative of a character in reserve by " + ReductionAmount + ".";
    }
}
