using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace
{
    public int XPos;
    public int YPos;

    public Creature Occupant = null;

    public bool Obstacle = false;

    public bool isBlocked => Obstacle || Occupant != null;
}
