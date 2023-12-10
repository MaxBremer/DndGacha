using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class PistolAbility : RangedAttackEnemiesAbility
{
    public PistolAbility()
    {
        Name = "Pistol";
        DisplayName = "Pistol";
        Range = 3;
        MaxCooldown = 0;
    }

    public override void UpdateDescription()
    {
        Description = "Ranged Attack: " + Range + ".";
    }
}
