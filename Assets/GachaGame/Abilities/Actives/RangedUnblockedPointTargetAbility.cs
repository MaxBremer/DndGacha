using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RangedUnblockedPointTargetAbility : RangedPointTargetAbility
{
    public override void InitAbility()
    {
        base.InitAbility();
        var targetChoice = ChoicesNeeded.Where(x => x.Caption == "Target").First() as PointTargetChoice;
        Func<GridSpace, bool> newIsValid = x => x != Owner.MySpace && !x.isBlocked && GachaGrid.IsInRange(Owner, x, Range);
        targetChoice.IsValidSpace = newIsValid;
    }
}
