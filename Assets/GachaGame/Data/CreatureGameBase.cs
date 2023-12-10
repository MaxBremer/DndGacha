using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureGameBase
{
    public string Name = "NONAME";

    public string DisplayName = "NONAME";

    public List<string> Abilities = new List<string>();

    public HashSet<CreatureTag> Tags = new HashSet<CreatureTag>();

    public HashSet<string> CreatureTypes = new HashSet<string>();

    public int Attack = 1;

    public int Health = 1;

    public int Speed = 1;

    public int Initiative = 1;

    public int[] AbilityRanks = new int[] { 0, 0, 0 };
}
