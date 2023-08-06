using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTargetChoice : Choice
{
    private GridSpace _targetSpace;
    
    public PointTargetChoice()
    {
        Type = ChoiceType.POINTTARGET;
    }

    public Func<GridSpace, bool> IsValidSpace = null;

    public GridSpace TargetSpace
    {
        get => _targetSpace;
        set
        {
            _targetSpace = value;
            ChoiceMade = _targetSpace != null;
        }
    }

    public override void ClearChoice()
    {
        TargetSpace = null;
    }
}
