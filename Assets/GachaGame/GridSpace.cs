using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace
{
    private bool _obstacle = false;

    public int XPos;
    public int YPos;

    public Creature Occupant = null;

    public bool Obstacle {
        get => _obstacle;
        set
        {
            _obstacle = value;
            if (_obstacle)
            {
                // TRIGGER OBSTACLEMADE EVENT OR SOMETHING
            }
        }
    }

    public bool isBlocked => Obstacle || Occupant != null;
}
