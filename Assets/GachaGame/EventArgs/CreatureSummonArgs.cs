using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSummonArgs : EventArgs
{
    public Creature BeingSummoned;

    public GridSpace LocationOfSummon;
}
