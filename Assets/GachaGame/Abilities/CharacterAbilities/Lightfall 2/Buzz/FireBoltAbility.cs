﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class FireBoltAbility : RangedAttackEnemiesAbility
{
    public FireBoltAbility()
    {
        Range = 2;
        Name = "FireBolt";
        DisplayName = "Fire Bolt";
    }

    public override void UpdateDescription()
    {
        Description = "Ranged Attack " + Range;
    }
}
