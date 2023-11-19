using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RarityRemovalDecorator : CharacterRollDecorator
{
    private Rarity _rarityToRemove;

    public RarityRemovalDecorator(ICharacterRoll decoratedRoll, Rarity rarityToRemove)
        : base(decoratedRoll)
    {
        _rarityToRemove = rarityToRemove;
    }

    public override Dictionary<Rarity, int> GetRarityWeights()
    {
        var weights = base.GetRarityWeights();
        weights.Remove(_rarityToRemove);
        return weights;
    }

    public override Dictionary<Rarity, List<ScriptableCharacterBase>> GetRollableCharacters()
    {
        var characters = base.GetRollableCharacters();
        characters.Remove(_rarityToRemove);
        return characters;
    }
}

