using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : Ability
{
    public override void RankUpToOne()
    {
        _defaultNumTriggers++;
    }

    public override void RankUpToTwo()
    {
        _defaultNumTriggers++;
    }
}
