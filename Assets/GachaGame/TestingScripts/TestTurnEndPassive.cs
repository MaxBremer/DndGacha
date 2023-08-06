using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTurnEndPassive : TurnEndPassive
{
    public override void Trigger(object sender, EventArgs e)
    {
        if (e is TurnEndArgs teArgs)
        {
            Debug.Log("End of turn event trigger. Cur player was: " + teArgs.PlayerWhoseTurnIsEnding);
            Debug.Log(teArgs.PlayerWhoseTurnIsEnding == Owner.Controller.MyPlayerIndex ? "It WAS my players turn" : "It WAS NOT my players turn");
        }
        Debug.Log("An end turn ability triggered.");
    }
}
