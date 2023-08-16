using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityDatabase
{
    private static string[] _abilsNotToRandomlyGet = new[]
    {
        "PointTargetObstacle",
    };

    public static Dictionary<string, Type> AbilityDictionary = new Dictionary<string, Type>()
    {
        // Dot
        { "ExpensiveArmor", typeof(ExpensiveArmorAbility) },
        { "VariableOffense", typeof(VariableOffenseAbility) },
        { "OverallCompetence", typeof(OverallCompetenceAbility) },

        // The Knight
        { "HelltechArmor", typeof(HelltechArmorAbility) },
        { "ArmCannon", typeof(ArmCannonAbility) },
        { "GargauthBlessing", typeof(GargauthBlessingAbility) },

        //The Husk
        { "LastAct", typeof(LastActAbility) },
        { "TollOfAges", typeof(TollOfAgesAbility) },
        { "ClothOfEras", typeof(ClothOfErasAbility) },

        // TEST ABILITIES
        { "PointTargetObstacle", typeof(PointTargetObstacle) },
    };

    // NOTE: DEPENDENCY ON TEXTUAL ORDER INITIALIZATION. Requires AbilityDictionary to be initialized first.
    public static List<string> ValidRandomAbilities = GetValidRandomAbilityDictionary();

    private static List<string> GetValidRandomAbilityDictionary()
    {
        var retList = new List<string>();
        retList.AddRange(AbilityDictionary.Keys);
        foreach (var remKey in _abilsNotToRandomlyGet)
        {
            retList.Remove(remKey);
        }

        return retList;
    }
}
