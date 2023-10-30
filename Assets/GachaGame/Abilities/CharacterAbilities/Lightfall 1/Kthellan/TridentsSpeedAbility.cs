using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TridentsSpeedAbility : MyTurnStartPassive
{
    public TridentsSpeedAbility()
    {
        Name = "TridentsSpeed";
        DisplayName = "Trident's Speed";
        Description = "At the start of your turn, swap this creatures speed and attack";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is TurnStartArgs)
        {
            var tempA = Owner.Attack;
            var tempS = Owner.Speed;
            Owner.StatsSet(AtkSet: tempS, SpeedSet: tempA);
        }
    }
}
