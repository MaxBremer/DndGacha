using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class SeizingOpportunityAbility : CreatureLeavesSpaceAbility
{
    public SeizingOpportunityAbility()
    {
        Name = "SeizingOpportunity";
        DisplayName = "Seizing Opportunity";
        Description = "If an enemy character within Range 1 of Hiro moves to a tile outside of Range 1 of Hiro, Hiro makes an attack on that creature before it moves.";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(e is CreatureSpaceArgs leaveArgs && leaveArgs.MyCreature.Controller != Owner.Controller && GachaGrid.IsInRange(Owner, leaveArgs.SpaceInvolved, 1))
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is CreatureSpaceArgs leaveArgs && leaveArgs.MyCreature != null)
        {
            Owner.AttackTarget(leaveArgs.MyCreature);
        }
    }
}
