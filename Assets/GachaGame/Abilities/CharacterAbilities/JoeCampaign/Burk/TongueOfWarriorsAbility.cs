using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class TongueOfWarriorsAbility : OrthogonalTargetEnemyAbility
{
    public TongueOfWarriorsAbility()
    {
        Name = "TongueOfWarriors";
        DisplayName = "Tongue of Warriors";
        Description = "Choose an orthogonal direction with a straight line to an enemy. Pull that enemy closer until it is 1 tile away. It takes damage for each tile it moved.";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Options").FirstOrDefault() is ConditionalOptionSelectChoice condOptChoice && condOptChoice.ChoiceMade)
        {
            var trueTarget = GetTargetForDir(condOptChoice.ChosenOption);


            var damageNum = GachaGrid.DistanceBetween(trueTarget, Owner) - 1;
            if(damageNum <= 0)
            {
                return;
            }
            GridSpace moveToSpace = null;
            var grid = Owner.MyGame.GameGrid;
            switch (condOptChoice.ChosenOption)
            {
                case "North":
                    moveToSpace = grid[(Owner.MySpace.XPos, Owner.MySpace.YPos + 1)];
                    break;
                case "South":
                    moveToSpace = grid[(Owner.MySpace.XPos, Owner.MySpace.YPos - 1)];
                    break;
                case "East":
                    moveToSpace = grid[(Owner.MySpace.XPos + 1, Owner.MySpace.YPos)];
                    break;
                case "West":
                    moveToSpace = grid[(Owner.MySpace.XPos - 1, Owner.MySpace.YPos)];
                    break;
            }

            grid.TeleportTo(trueTarget, moveToSpace);
            trueTarget.TakeDamage(damageNum, Owner);
        }
    }
}
