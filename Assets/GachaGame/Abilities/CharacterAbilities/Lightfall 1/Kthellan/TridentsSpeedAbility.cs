using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TridentsSpeedAbility : MyTurnStartPassive
{
    private int _turnsLeft = 6;

    public TridentsSpeedAbility()
    {
        Name = "TridentsSpeed";
        DisplayName = "Trident's Speed";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is TurnStartArgs)
        {
            if(AbilityRank < 1 || _turnsLeft > 0)
            {
                var tempA = Owner.Attack;
                var tempS = Owner.Speed;
                Owner.StatsSet(AtkSet: tempS, SpeedSet: tempA);
            }
            else
            {
                var toSet = Math.Max(Owner.Attack, Owner.Speed);
                Owner.StatsSet(AtkSet: toSet, SpeedSet: toSet);
            }

            if (AbilityRank > 0 && _turnsLeft > 0)
            {
                _turnsLeft--;
            }
        }
    }

    public override void UpdateDescription()
    {
        string endTag = AbilityRank < 1 || _turnsLeft < 1 ? "" : "After " + _turnsLeft + " more triggers, this sets speed and attack equal to the higher of the two.";
        Description = (_turnsLeft > 0 ? "At the start of your turn, swap this creatures speed and attack." : "At the start of your turn, set this creatures speed and attack to the higher of the two.") + endTag;
    }

    public override void RankUpToOne()
    {
    }

    public override void RankUpToTwo()
    {
        _turnsLeft -= 2;
    }
}
