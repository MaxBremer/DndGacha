using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TestRoller : MonoBehaviour
{
    public PlayerCollection collection;

    public ICharacterRoll currentRoll;

    [SerializeField] private Text resultDisplay; // Assign in the inspector
    [SerializeField] private Text weightDisplay;
    [SerializeField] private Text currentModifiersDisplay; // Assign in the inspector
    [SerializeField] private Dropdown BoostDropdown;

    // Start is called before the first frame update
    void Start()
    {
        collection = new PlayerCollection();
        ResetRoll();
    }

    private void ResetRoll()
    {
        currentRoll = new BasicCharacterRoll(); // Reset to basic roll
        UpdateCurrentModifiersDisplay();
    }

    public void AddBoost()
    {
        switch (BoostDropdown.value)
        {
            case 0:
                currentRoll = new RarityUpDecorator(currentRoll);
                break;
            case 1:
                currentRoll = new LevelUpDecorator(currentRoll);
                break;
            case 2:
                currentRoll = new RankUpDecorator(currentRoll);
                break;
            case 3:
                currentRoll = new CosmeticUpDecorator(currentRoll);
                break;
            case 4:
                currentRoll = new DupeCharacterDecorator(currentRoll, collection);
                break;
            case 5:
                currentRoll = new NewCharacterDecorator(currentRoll, collection);
                break;
            case 6:
                currentRoll = new LuckyRollDecorator(currentRoll);
                break;
            default:
                break;
        }

        UpdateCurrentModifiersDisplay();
    }

    // Implement similar methods for other decorators

    public void PerformRoll()
    {
        var result = currentRoll.MakeRoll(currentRoll);
        DisplayResult(result);
        collection.AddCharacter(result);
        ResetRoll(); // Reset after each roll
    }

    private void DisplayResult(CollectionCharacter result)
    {
        resultDisplay.text = "Rolled Character: " + (result != null ? result.CreatureBase.DisplayName : "None") + " (Rarity: " + result.Rarity.ToString() + ")\n\nRank: " + result.Rank + "   Level: " + result.Level + "   Cosmetic: " + result.Cosmetic.ToString();
    }

    private void UpdateCurrentModifiersDisplay()
    {
        var final = string.Empty;
        final += "Rarity weights: " + GetWeightString(currentRoll.GetRarityWeights().Select(x => x.Value).ToArray()) + "\n";
        final += "Rank weights: " + GetWeightString(currentRoll.GetRankWeights()) + "\n";
        final += "Level weights: " + GetWeightString(currentRoll.GetLevelWeights()) + "\n";
        final += "Cosmetic weights: " + GetWeightString(currentRoll.GetCosmeticWeights().Select(x => x.Value).ToArray()) + "\n";
        weightDisplay.text = final;
    }

    private string GetWeightString(int[] weights)
    {
        var stri = string.Empty;
        Array.ForEach(weights, x => stri += x + ", ");
        return stri;
    }
}
