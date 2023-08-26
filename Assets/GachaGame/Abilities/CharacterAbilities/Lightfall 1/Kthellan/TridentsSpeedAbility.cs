using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TridentsSpeedAbility : MyTurnStartPassive
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
            var tempSchange = Owner.Speed - Owner.Attack;
            var tempAchange = Owner.Attack - Owner.Speed;
            Owner.StatsChange(AtkChg: tempSchange, SpeedChg: tempAchange);
        }
    }
}
