using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableOffenseAbility : BeforeIAttackAbility
{
    private const int UPPER_END_ATK_RANGE = 5;

    //private int AttackGained = 0;

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
        var AttackGained = r.Next(1, UPPER_END_ATK_RANGE + 1);
        Owner.StatsChange(AtkChg: AttackGained);
        var tClass = new TempAtkRemover(AttackGained, Owner);
        EventManager.StartListening("AfterAttack", tClass.RemoveAttack);
    }
}

public class TempAtkRemover
{
    private int MyAtkAmount;
    private Creature Owner;
    public TempAtkRemover(int amount, Creature creat)
    {
        MyAtkAmount = amount;
        Owner = creat;
    }

    public void RemoveAttack(object sender, EventArgs e)
    {
        if(e is AttackArgs && sender is Creature c && c == Owner)
        {
            Owner.StatsChange(AtkChg: MyAtkAmount * -1);
            EventManager.StopListening("AfterAttack", RemoveAttack);
        }
    }
}
