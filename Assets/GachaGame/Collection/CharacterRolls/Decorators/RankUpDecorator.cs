using System;
using UnityEngine;

public class RankUpDecorator : CharacterRollDecorator
{
    private double _rankWeightFactor;

    public RankUpDecorator(ICharacterRoll decoratedRoll, double rankWeightFactor = 1.2) : base(decoratedRoll)
    {
        _rankWeightFactor = rankWeightFactor;
    }

    public override int[] GetRankWeights()
    {
        var baseWeights = base.GetRankWeights();
        var adjustedWeights = new int[baseWeights.Length];
        for (int i = 0; i < baseWeights.Length; i++)
        {
            adjustedWeights[i] = (int)(baseWeights[i] * Math.Pow(_rankWeightFactor, i));
        }

        return adjustedWeights;
    }
}

