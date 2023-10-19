using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PistolAbility : RangedAttackEnemiesAbility
{
    public PistolAbility()
    {
        Name = "Pistol";
        DisplayName = "Pistol";
        Description = "Ranged Attack: 3.";
        Range = 3;
        MaxCooldown = 0;
    }
}
