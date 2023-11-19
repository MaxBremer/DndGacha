using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCollection
{
    private Dictionary<CollectibleType, int> _collectibles = new Dictionary<CollectibleType, int>();

    public Dictionary<string, List<CollectionCharacter>> CharactersByName = new Dictionary<string, List<CollectionCharacter>>();

    public IEnumerable<CollectionCharacter> AllCharacters => CharactersByName.SelectMany(kvp => kvp.Value).AsEnumerable();

    public void AddCharacter(CollectionCharacter character)
    {
        var name = character.CreatureBase.Name;
        
        if (!CharactersByName.ContainsKey(name))
        {
            CharactersByName.Add(name, new List<CollectionCharacter>());
        }

        CharactersByName[name].Add(character);
    }

    public List<string> CharactersAvailableToRankUp()
    {
        const int MaxLevelForRankUp = 20;
        const int MaxRank = 3;

        var availableToRankUp = new List<string>();

        foreach (var entry in CharactersByName)
        {
            var rankGroups = entry.Value
                .Where(character => character.Level == MaxLevelForRankUp && character.Rank < MaxRank)
                .GroupBy(character => character.Rank)
                .Where(group => group.Count() >= 3);

            if (rankGroups.Any())
            {
                availableToRankUp.Add(entry.Key);
            }
        }

        return availableToRankUp;
    }

    public void AddCollectible(CollectibleType type, int quantity = 1)
    {
        if (_collectibles.ContainsKey(type))
        {
            _collectibles[type] += quantity;
        }
        else
        {
            _collectibles[type] = quantity;
        }
    }

    public int GetCollectibleCount(CollectibleType type)
    {
        if (_collectibles.TryGetValue(type, out int count))
        {
            return count;
        }
        return 0;
    }
}
