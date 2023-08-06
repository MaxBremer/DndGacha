using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureReservedArgs : EventArgs
{
    public Creature BeingReserved;

    public Player ReserveOwner;
}
