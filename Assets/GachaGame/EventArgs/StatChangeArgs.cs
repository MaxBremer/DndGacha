using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeArgs : EventArgs
{
    public int AttackChange;
    public int HealthChange;
    public int SpeedChange;
    public int InitChange;
}
