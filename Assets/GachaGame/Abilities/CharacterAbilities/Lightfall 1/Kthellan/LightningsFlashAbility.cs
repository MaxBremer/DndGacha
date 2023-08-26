using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LightningsFlashAbility : RangedPointTargetAbility
{
    public LightningsFlashAbility()
    {
        Name = "LightningsFlash";
        DisplayName = "Lightning's Flash";
        Description = "Teleport this unit to a space within range equal to its speed. Deala damage to adjacent units equal to this units attack.";
        MaxCooldown = 3;
        EventManager.StartListening("StartOfTurn", UpdateRange);
    }

    private void UpdateRange(object sender, EventArgs e)
    {
        Range = Owner.Speed;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target") is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            Owner.MyGame.GameGrid.TeleportTo(Owner, pointChoice.TargetSpace);
            foreach (var space in Owner.MyGame.GameGrid.GetAdjacents(pointChoice.TargetSpace, false))
            {
                if(space.Occupant != null)
                {
                    space.Occupant.TakeDamage(Owner.Attack, Owner);
                }
            }
        }
    }
}
