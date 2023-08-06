using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public List<ScriptableCharacterBase> CharacterList;

    public Dictionary<Rarity, List<ScriptableCharacterBase>> CharactersInRarity = new Dictionary<Rarity, List<ScriptableCharacterBase>>();

    public GameObject charDisplayObj;

    private CharacterDisplayController _mainDisplayController;

    private void Start()
    {
        SetupRarityTiers();
        _mainDisplayController = charDisplayObj.GetComponent<CharacterDisplayController>();
    }

    private void SetupRarityTiers()
    {
        foreach (var character in CharacterList)
        {
            if (!CharactersInRarity.ContainsKey(character.rarity))
            {
                CharactersInRarity.Add(character.rarity, new List<ScriptableCharacterBase>());
            }
            CharactersInRarity[character.rarity].Add(character);
        }

        foreach (var rar in CharactersInRarity.Keys)
        {
            Debug.Log("In " + StaticData.rarityNames[rar] + " tier: " + CharactersInRarity[rar].Count);
        }
    }

    public void DebugCharChoose()
    {
        Rarity chosenrare = CharacterRaritySelector.SelectRarityDefault();
        var targetCharList = CharactersInRarity[chosenrare];
        var chosen = targetCharList[Random.Range(0, targetCharList.Count)];

        _mainDisplayController.CharacterImage = chosen.CardTexture;

        _mainDisplayController.CharName = chosen.CharName;
    }

    public ScriptableCharacterBase OpenPackCharacter()
    {
        Rarity chosenrare = CharacterRaritySelector.SelectRarityDefault();
        var targetCharList = CharactersInRarity[chosenrare];
        var chosenChar = targetCharList[Random.Range(0, targetCharList.Count)];
        Debug.Log("Congrats! You obtained a(n) " + StaticData.rarityNames[chosenChar.rarity] + " character!");
        Debug.Log("You obtained: " + chosenChar.CharName);
        return chosenChar;
    }
}
