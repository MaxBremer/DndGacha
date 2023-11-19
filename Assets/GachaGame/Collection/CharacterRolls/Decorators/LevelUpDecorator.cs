using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpDecorator : CharacterRollDecorator
{
    private double _levelWeightFactor;

    public LevelUpDecorator(ICharacterRoll decoratedRoll, double levelWeightFactor = 1.2) : base(decoratedRoll)
    {
        _levelWeightFactor = levelWeightFactor;
    }

    public override int[] GetLevelWeights()
    {
        var baseWeights = base.GetLevelWeights();
        var adjustedWeights = new int[baseWeights.Length];

        for (int i = 0; i < baseWeights.Length; i++)
        {
            adjustedWeights[i] = (int)(baseWeights[i] * Math.Pow(_levelWeightFactor, i));
        }

        return adjustedWeights;
    }
}

