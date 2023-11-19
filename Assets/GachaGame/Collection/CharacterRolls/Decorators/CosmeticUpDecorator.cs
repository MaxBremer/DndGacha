using System;
using System.Collections.Generic;

public class CosmeticUpDecorator : CharacterRollDecorator
{
    private double _cosmeticWeightFactor;

    public CosmeticUpDecorator(ICharacterRoll decoratedRoll, double cosmeticWeightFactor = 1.1) : base(decoratedRoll)
    {
        _cosmeticWeightFactor = cosmeticWeightFactor;
    }

    public override Dictionary<CosmeticType, int> GetCosmeticWeights()
    {
        var baseWeights = base.GetCosmeticWeights();
        var adjustedWeights = new Dictionary<CosmeticType, int>();

        // Assuming the cosmetics are in ascending order of uniqueness
        int cosmeticIndex = 0;
        foreach (var kvp in baseWeights)
        {
            adjustedWeights[kvp.Key] = (int)(kvp.Value * Math.Pow(_cosmeticWeightFactor, cosmeticIndex));
            cosmeticIndex++;
        }

        return adjustedWeights;
    }
}

