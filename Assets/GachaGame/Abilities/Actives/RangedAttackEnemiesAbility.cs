using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackEnemiesAbility : RangedAttackAbility
{
    public override void InitAbility()
    {
        base.InitAbility();
        Func<Creature, bool> isValid = x => x != Owner && x.Controller != Owner.Controller && GachaGrid.IsInRange(Owner, x, Range);
        ChoicesNeeded.Add(new CreatureTargetChoice() { IsValidCreature = isValid, Caption = "Target" });
    }
}
