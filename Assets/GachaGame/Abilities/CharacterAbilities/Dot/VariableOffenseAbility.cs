using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableOffenseAbility : BeforeIAttackAbility
{
    private const int UPPER_END_ATK_RANGE = 5;

    private int AttackGained = 0;

    public VariableOffenseAbility()
    {
        Name = "VariableOffense";
        DisplayName = "Variable Offense";
        Description = "When this character attacks, gain 1 to " + UPPER_END_ATK_RANGE + " attack until after the attack.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        base.Trigger(sender, e);
        var r = new System.Random();
        AttackGained = r.Next(1, UPPER_END_ATK_RANGE + 1);
        Owner.StatsChange(AtkChg: AttackGained);
        EventManager.StartListening("AfterAttack", RemoveAttack);
    }

    private void RemoveAttack(object sender, EventArgs e)
    {
        if(e is AttackArgs && sender is Creature c && c == Owner)
        {
            Owner.StatsChange(AtkChg: AttackGained * -1);
            AttackGained = 0;
            EventManager.StopListening("AfterAttack", RemoveAttack);
        }
    }
}
