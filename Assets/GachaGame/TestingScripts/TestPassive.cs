using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPassive : PassiveAbility
{
    public override void AddReserveTriggers()
    {
        Debug.Log("Ran this trigger add!");
        EventManager.StartListening("TestTrigger", OnTestRun);
        EventManager.StartListening("TestAddingTrigger", CumulativeTestTrigger);
    }

    private void OnTestRun(object sender, EventArgs e)
    {
        Debug.Log("TRIGGERED");
        if(e is TestEventArgs ev)
        {
            Debug.Log("VAL: " + ev.ArgVal);
        }
    }

    private void CumulativeTestTrigger(object sender, EventArgs e)
    {
        if(e is TestEventArgs ev)
        {
            ev.ArgVal++;
            Debug.Log("Adding one!");
            Debug.Log("Current val: " + ev.ArgVal);
        }
    }
}

public class TestEventArgs : EventArgs
{
    public TestEventArgs(int val)
    {
        ArgVal = val;
    }

    public int ArgVal { get; set; }
}
