using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BasicCharacterRoll : ICharacterRoll
{
    private const double LEVEL_CURVE_POW = 1.3;

    private const double RARITY_WEIGHT_PROPORTION = 0.5;

    private static string[] SET_NAMES = new string[]
    {
        "Lightfall 1",
        "Lightfall 2",
        "Joe Campaign",
    };

    public Dictionary<Rarity, int> GetRarityWeights()
    {
        return new Dictionary<Rarity, int>
        {
            { Rarity.COMMON, 500 },
            { Rarity.RARE, 300 },
            { Rarity.EPIC, 150 },
            { Rarity.LEGENDARY, 40 },
            { Rarity.MYTHICAL, 10 }
        };
    }

    public Dictionary<CosmeticType, int> GetCosmeticWeights()
    {
        return new Dictionary<CosmeticType, int>
        {
            { CosmeticType.NORMAL, 76000 },
            { CosmeticType.SHINY, 22100 },
            { CosmeticType.ARTIFACT, 900 },
            { CosmeticType.COSMIC, 10 }
        };
    }

    public int[] GetRankWeights()
    {
        // Assuming a decreasing probability as the rank increases
        return new int[] { 60, 20, 10, 5 }; // Weights for ranks 0 to 3
    }

    public int[] GetLevelWeights()
    {
        int[] levelWeights = new int[20];
        double baseWeight = 100.0; // Starting weight for level 1

        for (int i = 0; i < levelWeights.Length; i++)
        {
            // Exponential decrease
            levelWeights[i] = Math.Max((int)(baseWeight / Math.Pow(LEVEL_CURVE_POW, i)), 1); // Adjust the divisor for steeper or shallower curves
        }
        return levelWeights;
    }

    public Dictionary<Rarity, List<ScriptableCharacterBase>> GetRollableCharacters()
    {
        var rollableCharactersByRarity = new Dictionary<Rarity, List<ScriptableCharacterBase>>();

        // Iterate through each expansion in the database
        foreach (var expansion in FullCharacterDatabase)
        {
            // Iterate through each rarity within the expansion
            foreach (var rarityGroup in expansion.Value)
            {
                // Check if the rarity is already in the dictionary
                if (!rollableCharactersByRarity.ContainsKey(rarityGroup.Key))
                {
                    rollableCharactersByRarity[rarityGroup.Key] = new List<ScriptableCharacterBase>();
                }

                // Add all characters of this rarity to the corresponding list in the dictionary
                rollableCharactersByRarity[rarityGroup.Key].AddRange(rarityGroup.Value);
            }
        }

        return rollableCharactersByRarity;
    }

    public (ScriptableCharacterBase cha, int weight)[] GetCharacterWeights(IEnumerable<ScriptableCharacterBase> Characters)
    {
        var retList = new List<(ScriptableCharacterBase cha, int weight)>();

        foreach (var guy in Characters)
        {
            retList.Add((guy, 1));
        }

        return retList.ToArray();
    }

    public virtual CollectionCharacter MakeRoll(ICharacterRoll getStatsFrom)
    {
        var rollableCharacters = getStatsFrom.GetRollableCharacters();
        if (rollableCharacters.Count == 0)
        {
            return null; // No characters to roll
        }

        var rarityWeights = getStatsFrom.GetRarityWeights();
        var characterWeights = getStatsFrom.GetCharacterWeights(rollableCharacters.Values.SelectMany(list => list)).ToArray();

        double totalWeight = characterWeights.Sum(item => item.weight * GetCombinedRarityWeight(rarityWeights, item.cha.rarity));
        double randomValue = UnityEngine.Random.Range(0, (float)totalWeight);
        double cumulativeWeight = 0;

        foreach (var (character, weight) in characterWeights)
        {
            cumulativeWeight += weight * GetCombinedRarityWeight(rarityWeights, character.rarity);

            if (randomValue < cumulativeWeight)
            {
                var newCharacter = new CollectionCharacter(character);

                // Roll for Level, Rank, and CosmeticType
                /*var levelToSet = RollForStat(getStatsFrom.GetLevelWeights(), true);
                while(newCharacter.Level < levelToSet)
                {
                    newCharacter.LevelUp();
                }*/
                newCharacter.Level = RollForStat(getStatsFrom.GetLevelWeights(), true);
                var rankToSet = RollForStat(getStatsFrom.GetRankWeights());
                while(newCharacter.Rank < rankToSet)
                {
                    newCharacter.RankUp();
                }
                newCharacter.Cosmetic = RollForCosmeticType(getStatsFrom.GetCosmeticWeights());

                return newCharacter;
            }
        }

        throw new InvalidOperationException("Failed to make a roll");
    }

    private double GetCombinedRarityWeight(Dictionary<Rarity, int> rarityWeights, Rarity rarity)
    {
        // Calculate the combined weight based on rarity and the proportionality constant
        return rarityWeights[rarity] * RARITY_WEIGHT_PROPORTION;
    }

    private int RollForStat(int[] weights, bool startFromOne = false)
    {
        int totalWeight = weights.Sum();
        int randomNumber = UnityEngine.Random.Range(0, totalWeight);
        int weightSum = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            weightSum += weights[i];
            if (randomNumber < weightSum)
                return startFromOne ? i + 1 : i;
        }

        throw new InvalidOperationException("Failed to roll for stat");
    }

    private CosmeticType RollForCosmeticType(Dictionary<CosmeticType, int> cosmeticWeights)
    {
        int totalWeight = cosmeticWeights.Sum(cw => cw.Value);
        int randomNumber = UnityEngine.Random.Range(0, totalWeight);
        int weightSum = 0;

        foreach (var cosmetic in cosmeticWeights)
        {
            weightSum += cosmetic.Value;
            if (randomNumber < weightSum)
                return cosmetic.Key;
        }

        throw new InvalidOperationException("Failed to roll for cosmetic type");
    }

    public static Dictionary<string, Dictionary<Rarity, List<ScriptableCharacterBase>>> FullCharacterDatabase { get; set; } = LoadAllCharacters();

    private static Dictionary<string, Dictionary<Rarity, List<ScriptableCharacterBase>>> LoadAllCharacters()
    {
        var allCharacters = new Dictionary<string, Dictionary<Rarity, List<ScriptableCharacterBase>>>();

        foreach (var expansionName in SET_NAMES)
        {
            var charactersInExpansion = Resources.LoadAll<ScriptableCharacterBase>($"Characters/{expansionName}");
            foreach (var character in charactersInExpansion.Where(x => !string.IsNullOrEmpty(x.CharDisplayName)))
            {
                if (!allCharacters.ContainsKey(expansionName))
                {
                    allCharacters[expansionName] = new Dictionary<Rarity, List<ScriptableCharacterBase>>();
                }

                if (!allCharacters[expansionName].ContainsKey(character.rarity))
                {
                    allCharacters[expansionName][character.rarity] = new List<ScriptableCharacterBase>();
                }

                allCharacters[expansionName][character.rarity].Add(character);
            }
        }

        return allCharacters;
    }
}
