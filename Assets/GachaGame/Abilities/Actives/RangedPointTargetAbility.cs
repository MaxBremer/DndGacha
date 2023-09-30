using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RangedPointTargetAbility : RangedActiveAbility
{
    public override void InitAbility()
    {
        base.InitAbility();
        Func<GridSpace, bool> isValid = x => x != Owner.MySpace && GachaGrid.IsInRange(Owner, x, Range);
        ChoicesNeeded.Add(new PointTargetChoice() { IsValidSpace = isValid, Caption = "Target" });
    }
}
