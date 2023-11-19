using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LuckyRollDecorator : CharacterRollDecorator
{
    private double _luckFactor;

    public LuckyRollDecorator(ICharacterRoll decoratedRoll, double luckFactor = 1.1) : base(decoratedRoll)
    {
        _luckFactor = luckFactor;  // Adjust this factor to control how much "luck" is added
    }

    public override Dictionary<Rarity, int> GetRarityWeights()
    {
        var baseWeights = base.GetRarityWeights();
        var maxWeight = baseWeights.Values.Max(); // Find the maximum weight

        var adjustedWeights = baseWeights.ToDictionary(
            kvp => kvp.Key,
            kvp => AdjustWeight(kvp.Value, maxWeight)
        );

        return adjustedWeights;
    }

    public override int[] GetRankWeights()
    {
        var baseWeights = base.GetRankWeights();
        var maxWeight = baseWeights.Max(); // Find the maximum weight

        var adjustedWeights = baseWeights.Select(w => AdjustWeight(w, maxWeight)).ToArray();

        return adjustedWeights;
    }

    public override int[] GetLevelWeights()
    {
        var baseWeights = base.GetLevelWeights();
        var maxWeight = baseWeights.Max(); // Find the maximum weight

        var adjustedWeights = baseWeights.Select(w => AdjustWeight(w, maxWeight)).ToArray();

        return adjustedWeights;
    }

    private int AdjustWeight(int weight, int maxWeight)
    {
        // Calculate the factor inversely proportional to the base weight
        double adjustmentFactor = _luckFactor + ((maxWeight - weight) / (double)maxWeight) * (_luckFactor - 1);
        return (int)(weight * adjustmentFactor);
    }

    public override Dictionary<CosmeticType, int> GetCosmeticWeights()
    {
        var baseWeights = base.GetCosmeticWeights();
        var maxWeight = baseWeights.Values.Max(); // Find the maximum weight

        var adjustedWeights = baseWeights.ToDictionary(
            kvp => kvp.Key,
            kvp => AdjustWeight(kvp.Value, maxWeight)
        );

        return adjustedWeights;
    }
}

