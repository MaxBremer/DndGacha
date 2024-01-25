using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class SeizingOpportunityAbility : CreatureLeavesSpaceAbility
{
    private int _numAtks = 1;
    private int _reactRange = 1;

    public SeizingOpportunityAbility()
    {
        Name = "SeizingOpportunity";
        DisplayName = "Seizing Opportunity";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(e is CreatureSpaceArgs leaveArgs && leaveArgs.MyCreature.Controller != Owner.Controller && GachaGrid.IsInRange(Owner, leaveArgs.SpaceInvolved, _reactRange))
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is CreatureSpaceArgs leaveArgs && leaveArgs.MyCreature != null)
        {
            for (int i = 0; i < _numAtks; i++)
            {
                Owner.AttackTarget(leaveArgs.MyCreature);
            }
        }
    }

    public override void UpdateDescription()
    {
        string atkBit = _numAtks == 1 ? "an attack" : _numAtks + " attacks";
        Description = "If an enemy character within Range " + _reactRange + " of Hiro moves to another tile, Hiro makes " + atkBit + " on that creature before it moves.";
    }

    public override void RankUpToOne()
    {
        _numAtks++;
    }

    public override void RankUpToTwo()
    {
        _reactRange++;
    }
}
