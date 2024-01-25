using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class FoldingRealityAbility : AnyUnblockedPointTargetAbility
{
    public FoldingRealityAbility()
    {
        Name = "FoldingReality";
        DisplayName = "Folding Reality";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            Owner.MyGame.GameGrid.TeleportTo(Owner, pointChoice.TargetSpace);
            if (AbilityRank == 2)
            {
                var targetSquares = Owner.MyGame.GameGrid.GetAdjacents(pointChoice.TargetSpace, false);
                foreach (var sq in targetSquares)
                {
                    if(sq.Occupant != null && sq.Occupant.Controller != Owner.Controller)
                    {
                        Owner.AttackTarget(sq.Occupant, true);
                    }
                }
            }
        }
    }

    public override void UpdateDescription()
    {
        Description = "Teleport to any tile on the board." + (AbilityRank < 2 ? "" : " Perform a ranged attack on any enemies adjacent to the tile.");
    }

    public override void RankUpToTwo()
    {
    }
}
