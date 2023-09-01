using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LimbOfChainsAbility : RangedAttackEnemiesAbility
{
    public LimbOfChainsAbility()
    {
        Name = "LimbOfChains";
        DisplayName = "Limb of Chains";
        Description = "Ranged Attack 1";
        MaxCooldown = 0;
        Range = 1;
    }
}
