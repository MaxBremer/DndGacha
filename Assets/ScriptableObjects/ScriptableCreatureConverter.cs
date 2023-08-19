using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScriptableCreatureConverter
{
    public static CreatureGameBase GameBaseFromScriptableCharacter(ScriptableCharacterBase scb)
    {
        var ret = new CreatureGameBase()
        {
            Attack = scb.Attack,
            Health = scb.Health,
            Initiative = scb.Initiative,
            Speed = scb.Speed,
            Name = scb.CharName,
            DisplayName = scb.CharDisplayName,
            Abilities = new List<string>(),
            Tags = new HashSet<CreatureTag>(),
        };
        ret.Abilities.AddRange(scb.Abilities);
        foreach (var tag in scb.Tags)
        {
            ret.Tags.Add(tag);
        }

        return ret;
    }
}
