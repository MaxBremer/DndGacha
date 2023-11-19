using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewCharacterDecorator : CharacterRollDecorator
{
    private PlayerCollection _playerCollection;

    public NewCharacterDecorator(ICharacterRoll decoratedRoll, PlayerCollection playerCollection)
        : base(decoratedRoll)
    {
        _playerCollection = playerCollection;
    }

    public override Dictionary<Rarity, List<ScriptableCharacterBase>> GetRollableCharacters()
    {
        var rollableCharacters = base.GetRollableCharacters();
        var ownedCharacterNames = new HashSet<string>(_playerCollection.AllCharacters.Select(cc => cc.CreatureBase.Name));

        foreach (var rarityList in rollableCharacters.Values)
        {
            rarityList.RemoveAll(character => ownedCharacterNames.Contains(character.CharName));
        }

        var raritiesWithNoCharacters = rollableCharacters.Where(kvp => !kvp.Value.Any()).Select(kvp => kvp.Key).ToList();
        foreach (var rarity in raritiesWithNoCharacters)
        {
            rollableCharacters.Remove(rarity);
        }

        return rollableCharacters;
    }
}

