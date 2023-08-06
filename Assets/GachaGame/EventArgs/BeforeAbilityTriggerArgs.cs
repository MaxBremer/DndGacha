using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeAbilityTriggerArgs : EventArgs
{
    public int NumberOfTriggers = 1;
    public bool Countered = false;
}
