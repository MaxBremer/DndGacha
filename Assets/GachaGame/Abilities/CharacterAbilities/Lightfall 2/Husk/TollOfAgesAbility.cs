using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TollOfAgesAbility : MyTurnEndPassive
{
    private const int DAMAGE_AMOUNT = 2;

    public TollOfAgesAbility()
    {
        Name = "TollOfAges";
        DisplayName = "Toll of the Ages";
        Description = "At the end of your turn, this character takes " + DAMAGE_AMOUNT + " damage.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.TakeDamage(DAMAGE_AMOUNT, Owner);
    }
}
