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
    }

    public override void InitAbility()
    {
        MaxCooldown = 6;
        Range = 6;
        base.InitAbility();
    }
}
