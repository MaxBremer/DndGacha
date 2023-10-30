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
        Description = "Teleport to any tile on the board";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            Owner.MyGame.GameGrid.TeleportTo(Owner, pointChoice.TargetSpace);
        }
    }
}
