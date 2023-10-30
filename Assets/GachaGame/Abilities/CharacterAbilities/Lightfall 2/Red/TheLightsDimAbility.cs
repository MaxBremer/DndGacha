using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class TheLightsDimAbility : RangedUnblockedPointTargetAbility
{
    public TheLightsDimAbility()
    {
        Name = "TheLightsDim";
        DisplayName = "The Lights Dim...";
        Description = "Choose a point in Range 3. Move this character to it. If \"Showtime\" was used last turn, restore this character to full health.";
        MaxCooldown = 1;
        Range = 3;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            Owner.MyGame.GameGrid.TeleportTo(Owner, pointChoice.TargetSpace);

            if (GameUtils.AbilWasTriggeredNTurnsAgo(1, "Showtime"))
            {
                Owner.Heal(Owner.MaxHealth);
            }
        }
    }
}
