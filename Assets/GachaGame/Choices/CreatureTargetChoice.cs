using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureTargetChoice : Choice
{
    private Creature _targetCreature;

    public CreatureTargetChoice()
    {
        Type = ChoiceType.CREATURETARGET;
    }

    public Func<Creature, bool> IsValidCreature;

    public Creature TargetCreature
    {
        get => _targetCreature;
        set
        {
            _targetCreature = value;
            ChoiceMade = _targetCreature != null;
        }
    }

    public override void ClearChoice()
    {
        TargetCreature = null;
    }
}
