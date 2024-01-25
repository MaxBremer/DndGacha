using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class SeverFromRealityAbility : RangedAttackEnemiesAbility
{
    public SeverFromRealityAbility()
    {
        Name = "SeverFromReality";
        DisplayName = "Sever from Reality";
        MaxCooldown = 1;
        Range = 3;
    }

    public override void UpdateDescription()
    {
        Description = "Ranged Attack " + Range;
    }

    public override void RankUpToTwo()
    {
        MaxCooldown = Math.Max(0, MaxCooldown - 1);
    }
}
