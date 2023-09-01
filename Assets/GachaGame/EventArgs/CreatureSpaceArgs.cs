using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpaceArgs : EventArgs
{
    public Creature MyCreature;
    public GridSpace SpaceInvolved;

    public CreatureSpaceArgs CreateCopy()
    {
        return new CreatureSpaceArgs() { MyCreature = MyCreature, SpaceInvolved = SpaceInvolved };
    }
}
