using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FubikianLongbowAbility : RangedAttackEnemiesAbility
{
    public FubikianLongbowAbility()
    {
        Name = "FubikianLongbow";
        DisplayName = "Fubikian Longbow";
        Description = "Ranged Attack 3";
        MaxCooldown = 0;
        Range = 3;
    }
}
