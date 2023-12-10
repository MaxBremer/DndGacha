using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TollOfAgesAbility : MyTurnEndPassive
{
    private int DAMAGE_AMOUNT = 2;
    private bool doThisTurn = true;

    public TollOfAgesAbility()
    {
        Name = "TollOfAges";
        DisplayName = "Toll of the Ages";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(AbilityRank < 1 || doThisTurn)
        {
            Owner.TakeDamage(DAMAGE_AMOUNT, Owner);
        }

        if(AbilityRank >= 1)
        {
            doThisTurn = !doThisTurn;
        }
    }

    public override void RankUpToOne()
    {
    }

    public override void RankUpToTwo()
    {
        DAMAGE_AMOUNT--;
    }

    public override void UpdateDescription()
    {
        string turnIndicator = AbilityRank < 1 ? "your turn" : "every other one of your turns";
        Description = "At the end of " + turnIndicator + ", this character takes " + DAMAGE_AMOUNT + " damage.";
    }
}
