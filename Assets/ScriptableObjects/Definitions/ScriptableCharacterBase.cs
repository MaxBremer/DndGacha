using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterBase", menuName = "DndGacha/CharacterBase", order = 1)]
public class ScriptableCharacterBase : ScriptableObject
{
    // Basic character-descriptive fields.
    public string CharName;
    public string CharDisplayName;
    public string FlavorText;
    public Rarity rarity;

    // Unity-specific, likely to change later, for storing image. Especially likely to change given multiple skins.
    public Texture CardTexture;

    // Basic in-game stats.
    public int Attack;
    public int Health;
    public int Speed;
    public int Initiative;

    // Abilities by Name field.
    public List<string> Abilities;
    public List<CreatureTag> Tags;
    // TODO: Change to CreatureType enum.
    public List<string> CreatureTypes;

    // Collection fields.
    public LevelingCurveType LevelCurveType = LevelingCurveType.NULL;

}
