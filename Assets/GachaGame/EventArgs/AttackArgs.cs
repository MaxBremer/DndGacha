using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArgs : EventArgs
{
    public Creature Target;
    public bool IsRanged;
}
