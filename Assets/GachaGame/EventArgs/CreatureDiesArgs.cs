using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDiesArgs : EventArgs
{
    public Creature CreatureDied;
    public GridSpace WhereItDied;
}
