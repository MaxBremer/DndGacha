using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData
{
    public static Dictionary<Rarity, string> rarityNames = new Dictionary<Rarity, string>()
    {
        {Rarity.COMMON, "Common" },
        {Rarity.RARE, "Rare" },
        {Rarity.EPIC, "Epic" },
        {Rarity.LEGENDARY, "Legendary" },
        {Rarity.MYTHICAL, "Mythical" },
    };
}

public enum Rarity
{
    COMMON,
    RARE,
    EPIC,
    LEGENDARY,
    MYTHICAL
}
