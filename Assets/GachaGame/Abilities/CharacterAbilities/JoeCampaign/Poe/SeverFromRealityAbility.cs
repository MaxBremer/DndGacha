using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SeverFromRealityAbility : RangedAttackEnemiesAbility
{
    public SeverFromRealityAbility()
    {
        Name = "SeverFromReality";
        DisplayName = "Sever from Reality";
        Description = "Ranged Attack 3";
        MaxCooldown = 1;
        Range = 3;
    }
}
