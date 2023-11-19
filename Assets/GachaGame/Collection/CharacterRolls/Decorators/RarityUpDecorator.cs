using System;
using System.Collections.Generic;

public class RarityUpDecorator : CharacterRollDecorator
{
    private double _rarityWeightFactor;

    public RarityUpDecorator(ICharacterRoll decoratedRoll, double rarityWeightFactor = 1.1) : base(decoratedRoll)
    {
        _rarityWeightFactor = rarityWeightFactor;
    }

    public override Dictionary<Rarity, int> GetRarityWeights()
    {
        var baseWeights = base.GetRarityWeights();
        var adjustedWeights = new Dictionary<Rarity, int>();

        // Assuming the rarities are in ascending order of rarity
        int rarityIndex = 0;
        foreach (var kvp in baseWeights)
        {
            adjustedWeights[kvp.Key] = (int)(kvp.Value * Math.Pow(_rarityWeightFactor, rarityIndex));
            rarityIndex++;
        }

        return adjustedWeights;
    }
}

