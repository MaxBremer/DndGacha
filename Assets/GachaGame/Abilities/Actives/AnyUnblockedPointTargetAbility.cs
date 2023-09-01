using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AnyUnblockedPointTargetAbility : ActiveAbility
{
    public override void InitAbility()
    {
        base.InitAbility();
        Func<GridSpace, bool> isValid = x => !x.isBlocked;
        ChoicesNeeded.Add(new PointTargetChoice() { Caption = "Target", IsValidSpace = isValid });
    }
}
