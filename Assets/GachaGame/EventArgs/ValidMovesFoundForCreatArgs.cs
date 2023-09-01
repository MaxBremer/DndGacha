using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidMovesFoundForCreatArgs : EventArgs
{
    public Dictionary<GridSpace, List<GridSpace>> ValidMovesWithPaths;
    public Creature CreatureMoving;
}
