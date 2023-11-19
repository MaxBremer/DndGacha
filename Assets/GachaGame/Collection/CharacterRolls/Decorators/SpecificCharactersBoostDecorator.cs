using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecificCharactersBoostDecorator : CharacterRollDecorator
{
    private HashSet<string> _boostedCharacterNames;
    private double _boostFactor;

    public SpecificCharactersBoostDecorator(ICharacterRoll decoratedRoll, IEnumerable<string> boostedCharacterNames, double boostFactor = 2)
        : base(decoratedRoll)
    {
        _boostedCharacterNames = new HashSet<string>(boostedCharacterNames);
        _boostFactor = boostFactor;  // Adjust this factor to control how much boost is applied
    }

    public override (ScriptableCharacterBase cha, int weight)[] GetCharacterWeights(IEnumerable<ScriptableCharacterBase> characters)
    {
        var baseWeights = base.GetCharacterWeights(characters);
        var adjustedWeights = baseWeights.Select(cw =>
        {
            if (_boostedCharacterNames.Contains(cw.cha.CharName))
            {
                return (cw.cha, (int)(cw.weight * _boostFactor));
            }
            return cw;
        }).ToArray();

        return adjustedWeights;
    }

    // Other methods remain unchanged
}
