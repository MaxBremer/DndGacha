using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityDatabase
{
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
    };
}
