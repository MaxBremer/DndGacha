using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class VariableOffenseAbility : BeforeIAttackAbility
{
    private int UPPER_END_ATK_RANGE = 5;

    //private int AttackGained = 0;

    public VariableOffenseAbility()
    {
        Name = "VariableOffense";
        DisplayName = "Variable Offense";
    }

    //TODO: Turn attack remover into ability?
    public override void Trigger(object sender, EventArgs e)
    {
        if(e is AttackArgs atkArgs)
        {
            var r = new System.Random();
            var AttackGained = r.Next(1, UPPER_END_ATK_RANGE + 1);
            //TODO: What if damage nullified before this trigger? This completely undoes that. Figure out solution.
            Owner.StatsChange(AtkChg: AttackGained);
            atkArgs.DamageToDeal = Owner.Attack;
            var tClass = new TempAtkRemover(AttackGained, Owner) { Priority = Priority, };
            EventManager.StartListening(GachaEventType.AfterAttack, tClass.RemoveAttack, Priority);
        }
        
    }

    public override void UpdateDescription()
    {
        Description = "When this character attacks, gain 1 to " + UPPER_END_ATK_RANGE + " attack until after the attack.";
    }

    public override void RankUpToOne()
    {
        UPPER_END_ATK_RANGE += 2;
    }

    public override void RankUpToTwo()
    {
        UPPER_END_ATK_RANGE += 2;
    }
}

public class TempAtkRemover : PassiveAbility
{
    private int MyAtkAmount;
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
            EventManager.StopListening(GachaEventType.AfterAttack, RemoveAttack, Priority);
        }
    }
}
