using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableCharacterBase : ScriptableObject
{
    public string CharName;
    public string CharDisplayName;
    public string FlavorText;
    public Rarity rarity;

    public Texture CardTexture;

    public int Attack;
    public int Health;
    public int Speed;
    public int Initiative;

    public List<string> Abilities;
    public List<CreatureTag> Tags;
}
