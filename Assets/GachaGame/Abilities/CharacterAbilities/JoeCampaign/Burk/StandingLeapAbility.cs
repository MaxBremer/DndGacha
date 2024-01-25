using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class StandingLeapAbility : ActiveAbility
{
    private int JUMP_DIST = 3;

    public StandingLeapAbility()
    {
        Name = "StandingLeap";
        DisplayName = "Standing Leap";
        MaxCooldown = 0;
    }

    public override bool IsActivateable()
    {
        // TODO: Better way of checking if have moved
        return base.IsActivateable() && Owner.SpeedLeft == Owner.Speed;
    }

    public override void InitAbility()
    {
        base.InitAbility();
        Func<GridSpace, bool> isValid = x =>
        {
            return (!x.isBlocked) && 
            ((x.XPos == Owner.MySpace.XPos + JUMP_DIST && x.YPos == Owner.MySpace.YPos) || 
            (x.XPos == Owner.MySpace.XPos - JUMP_DIST && x.YPos == Owner.MySpace.YPos) || 
            (x.XPos == Owner.MySpace.XPos && x.YPos == Owner.MySpace.YPos + JUMP_DIST) ||
            (x.XPos == Owner.MySpace.XPos && x.YPos == Owner.MySpace.YPos - JUMP_DIST));
        };
        ChoicesNeeded.Add(new PointTargetChoice() { Caption = "Target", IsValidSpace = isValid, });
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            Owner.MyGame.GameGrid.TeleportTo(Owner, pointChoice.TargetSpace);
            // TODO: Make temp CANT_MOVE tag.
            // TODO: Simplify process for temporary tags.
            Owner.SpeedLeft = 0;
        }
    }

    public override void UpdateDescription()
    {
        Description = "If this creature has not moved yet this turn, it may instead move exactly " + JUMP_DIST + " spaces in a straight line. This move is treated as Flying.";
    }

    public override void RankUpToOne()
    {
        JUMP_DIST++;
    }

    public override void RankUpToTwo()
    {
        JUMP_DIST++;
    }
}
