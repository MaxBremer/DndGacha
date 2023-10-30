using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class LightningsFlashAbility : RangedPointTargetAbility
{
    public LightningsFlashAbility()
    {
        Name = "LightningsFlash";
        DisplayName = "Lightning's Flash";
        Description = "Teleport this unit to a space within range equal to its speed. Deals damage to adjacent units equal to this units attack.";
        MaxCooldown = 3;
    }

    public override bool IsActivateable()
    {
        Range = Owner.Speed;
        return base.IsActivateable();
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").First() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            Owner.MyGame.GameGrid.TeleportTo(Owner, pointChoice.TargetSpace);
            foreach (var space in Owner.MyGame.GameGrid.GetAdjacents(Owner.MySpace, false))
            {
                if(space.Occupant != null)
                {
                    space.Occupant.TakeDamage(Owner.Attack, Owner);
                }
            }
        }
    }
}
