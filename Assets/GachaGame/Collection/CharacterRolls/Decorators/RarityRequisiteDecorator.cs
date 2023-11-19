using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RarityRequisiteDecorator : CharacterRollDecorator
{
    private Rarity _requiredRarity;

    public RarityRequisiteDecorator(ICharacterRoll decoratedRoll, Rarity requiredRarity)
        : base(decoratedRoll)
    {
        _requiredRarity = requiredRarity;
    }

    public override Dictionary<Rarity, int> GetRarityWeights()
    {
        var weights = new Dictionary<Rarity, int>
        {
            { _requiredRarity, base.GetRarityWeights()[_requiredRarity] }
        };
        return weights;
    }

    public override Dictionary<Rarity, List<ScriptableCharacterBase>> GetRollableCharacters()
    {
        var characters = base.GetRollableCharacters();
        return new Dictionary<Rarity, List<ScriptableCharacterBase>>
        {
            { _requiredRarity, characters.ContainsKey(_requiredRarity) ? characters[_requiredRarity] : new List<ScriptableCharacterBase>() }
        };
    }
}

