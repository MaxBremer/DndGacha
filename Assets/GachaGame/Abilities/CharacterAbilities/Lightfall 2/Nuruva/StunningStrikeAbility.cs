using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StunningStrikeAbility : TouchRangeEnemyCreatureAbility
{
    public StunningStrikeAbility()
    {
        Name = "StunningStrike";
        DisplayName = "Stunning Strike";
        Description = "Attack. That target is Stunned, and cannot act or move on its next turn.";
        MaxCooldown = 3;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            Owner.AttackTarget(creatChoice.TargetCreature);
            creatChoice.TargetCreature.GainTag(CreatureTag.STUNNED);
        }
    }
}
