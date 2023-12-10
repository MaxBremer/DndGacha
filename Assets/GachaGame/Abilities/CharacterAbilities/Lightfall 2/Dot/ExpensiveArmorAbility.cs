using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ExpensiveArmorAbility : BeforeITakeDamagePassiveAbility
{
    private int reductionAmount = 1;

    public ExpensiveArmorAbility()
    {
        Name = "ExpensiveArmor";
        DisplayName = "Expensive Armor";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is TakingDamageArgs dmgE)
        {
            dmgE.DamageAmount = Math.Max(0, dmgE.DamageAmount - 1);
            if(AbilityRank >= 1)
            {
                Owner.StatsChange(AtkChg: 1);
            }
        }
    }

    public override void UpdateDescription()
    {
        string suffix = AbilityRank < 1 ? "." : " and gain 1 attack.";
        Description = "When this character takes damage, reduce it by " + reductionAmount + suffix;
    }

    public override void RankUpToOne()
    {
        
    }

    public override void RankUpToTwo()
    {
        reductionAmount++;
    }
}
