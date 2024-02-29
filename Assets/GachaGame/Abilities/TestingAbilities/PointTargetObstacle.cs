using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointTargetObstacle : ActiveAbility
{
    public PointTargetObstacle()
    {
        Name = "PointTargetObstacle";
        DisplayName = "Test Point";
        Description = "C0: Make a space an obstacle.";
    }

    public override void InitAbility()
    {
        base.InitAbility();
        MaxCooldown = 0;
        Func<GridSpace, bool> isValid = gs => (!gs.isBlocked) && GachaGrid.IsInRange(Owner, gs, 4);
        ChoicesNeeded.Add(new PointTargetChoice() { IsValidSpace = isValid, Caption = "Target1" });
        ChoicesNeeded.Add(new OptionSelectChoice(new List<string>() { "Only one", "Second one" }));

        Func<GridSpace, bool> secondIsValid = gs => (!gs.isBlocked) && GachaGrid.IsInRange(Owner, gs, 2) && gs != (ChoicesNeeded[0] as PointTargetChoice).TargetSpace;
        ChoicesNeeded.Add(new PointTargetChoice() { IsValidSpace = secondIsValid, Caption = "Target2" });
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target1").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            pointChoice.TargetSpace.Obstacle = true;
        }

        if (ChoicesNeeded.Where(x => x.Caption == "Target2").FirstOrDefault() is PointTargetChoice pointChoice2 && pointChoice2.ChoiceMade && ChoicesNeeded[1] is OptionSelectChoice optSel && optSel.ChosenOptionString == "Second one")
        {
            pointChoice2.TargetSpace.Obstacle = true;
        }
    }
}
