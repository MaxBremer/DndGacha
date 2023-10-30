using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ExpensiveArmorAbility : BeforeITakeDamagePassiveAbility
{
    public ExpensiveArmorAbility()
    {
        Name = "ExpensiveArmor";
        DisplayName = "Expensive Armor";
        Description = "When this character takes damage, reduce it by 1.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is TakingDamageArgs dmgE)
        {
            dmgE.DamageAmount = Math.Max(0, dmgE.DamageAmount - 1);
        }
    }
}
