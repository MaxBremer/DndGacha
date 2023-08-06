using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTurnEndPassive : TurnEndPassive
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(e is TurnEndArgs turnArgs && turnArgs.PlayerWhoseTurnIsEnding == Owner.Controller.MyPlayerIndex)
        {
            ExternalTrigger(sender, e);
        }
    }
}
