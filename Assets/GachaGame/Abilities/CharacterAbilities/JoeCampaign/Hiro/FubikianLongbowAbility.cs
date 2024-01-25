using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class FubikianLongbowAbility : RangedAttackEnemiesAbility
{
    public FubikianLongbowAbility()
    {
        Name = "FubikianLongbow";
        DisplayName = "Fubikian Longbow";
        MaxCooldown = 0;
        Range = 3;
    }

    public override void UpdateDescription()
    {
        Description = "Ranged Attack " + Range;
    }
}
