using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RangedMultiplePointTargetAbility : ActiveAbility
{
    public int Range = 0;
    public int NumTargets = 1;

    public override void InitAbility()
    {
        base.InitAbility();

        Func<GridSpace, bool> isValid = x => (!ChoicesNeeded.Where(y => y is PointTargetChoice pointChoice && pointChoice.TargetSpace == x).Any()) && x != Owner.MySpace && !x.isBlocked && GachaGrid.IsInRange(Owner, x, Range);
        for (int i = 0; i < NumTargets; i++)
        {
            var num = i + 1;
            ChoicesNeeded.Add(new PointTargetChoice() { IsValidSpace = isValid, Caption = "Target" + num });
        }

        //Func<GridSpace, bool> isValid = x => x != Owner.MySpace && !x.isBlocked && GachaGrid.IsInRange(Owner, x, Range);
        //ChoicesNeeded.Add(new PointTargetChoice() { IsValidSpace = isValid, Caption = "Target" });
    }

    public override bool IsActivateable()
    {
        return base.IsActivateable() && ChoiceManager.NumValidChoicesExist(ChoicesNeeded[0], this, NumTargets);
    }
}
