using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class HoldPersonAbility : RangedTargetEnemyAbility
{
    private int _healthCeiling = 10;

    public HoldPersonAbility()
    {
        Name = "HoldPerson";
        DisplayName = "Hold Person";
        Range = 3;
        MaxCooldown = 1;
    }

    public override void InitAbility()
    {
        base.InitAbility();

        var targetChoice = ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault();
        if(targetChoice != null && targetChoice is CreatureTargetChoice creatChoice)
        {
            Func<Creature, bool> newIsValid = x => x.IsOnBoard && x.Controller != Owner.Controller && GachaGrid.IsInRange(Owner, x, Range) && x.Health < _healthCeiling;
            creatChoice.IsValidCreature = newIsValid;
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)// && !creatChoice.TargetCreature.HasTag(CreatureTag.CANT_MOVE))
        {
            creatChoice.TargetCreature.GainTag(CreatureTag.CANT_MOVE);
            creatChoice.TargetCreature.GainHiddenAbility(new RemoveCantMoveEndOfTurn());
        }
    }

    public override void UpdateDescription()
    {
        Description = "Choose a character below " + _healthCeiling + " health within range " + Range + ". They cannot move on their next turn.";
    }

    public override void RankUpToOne()
    {
        _healthCeiling += 4;
    }

    public override void RankUpToTwo()
    {
        Range++;
        MaxCooldown = Math.Max(0, MaxCooldown - 1);
    }
}

public class RemoveCantMoveEndOfTurn : MyTurnEndPassive
{
    public RemoveCantMoveEndOfTurn()
    {
        Name = "PersonHeld";
        DisplayName = "Held";
        Description = "Cannot move until the end of my next turn.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.LoseTag(CreatureTag.CANT_MOVE);
        Owner.RemoveHiddenAbility(this);
    }
}
