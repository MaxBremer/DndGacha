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
            Tags = new List<CreatureTag>(),
        };
        ret.Abilities.AddRange(scb.Abilities);
        ret.Tags.AddRange(scb.Tags);

        return ret;
    }
}
