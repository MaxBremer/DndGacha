using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFriendlyOrSelfAbility : ActiveAbility
{
    public override void InitAbility()
    {
        base.InitAbility();
        Func<Creature, bool> isValid = c => c.IsOnBoard && c.Controller == Owner.Controller;
        ChoicesNeeded.Add(new CreatureTargetChoice() { IsValidCreature = isValid, Caption = "Target" });
    }
}
