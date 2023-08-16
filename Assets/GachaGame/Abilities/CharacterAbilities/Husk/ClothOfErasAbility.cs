using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClothOfErasAbility : RangedAttackEnemiesAbility
{
    public ClothOfErasAbility()
    {
        Name = "ClothOfEras";
        DisplayName = "Cloth of Eras";
        Description = "Ranged Attack: 6";
        MaxCooldown = 0;
    }

    public override void InitAbility()
    {
        Range = 6;
        base.InitAbility();
    }
}
