using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class LimbOfChainsAbility : RangedAttackEnemiesAbility
{
    public LimbOfChainsAbility()
    {
        Name = "LimbOfChains";
        DisplayName = "Limb of Chains";
        MaxCooldown = 0;
        Range = 1;
    }

    public override void UpdateDescription()
    {
        Description = "Ranged Attack " + Range;
    }
}
