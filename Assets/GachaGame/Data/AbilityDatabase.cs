using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityDatabase
{
    private static string[] _abilsNotToRandomlyGet = new[]
    {
        "PointTargetObstacle",
        "TragicBackstory",
    };

    public static Dictionary<string, Type> AbilityDictionary = new Dictionary<string, Type>()
    {
        // CAMPAIGN 2
        // Dot
        { "ExpensiveArmor", typeof(ExpensiveArmorAbility) },
        { "VariableOffense", typeof(VariableOffenseAbility) },
        { "OverallCompetence", typeof(OverallCompetenceAbility) },

        // The Knight
        { "HelltechArmor", typeof(HelltechArmorAbility) },
        { "ArmCannon", typeof(ArmCannonAbility) },
        { "GargauthBlessing", typeof(GargauthBlessingAbility) },

        // The Husk
        { "LastAct", typeof(LastActAbility) },
        { "TollOfAges", typeof(TollOfAgesAbility) },
        { "ClothOfEras", typeof(ClothOfErasAbility) },

        // Nuruva
        { "FlurryOfBlows", typeof(FlurryOfBlowsAbility) },
        { "FocusKi", typeof(FocusKiAbility) },
        { "StunningStrike", typeof(StunningStrikeAbility) },

        // Rasuil
        { "AuraOfCourage", typeof(AuraOfCourageAbility) },
        { "DivineSmite", typeof(DivineSmiteAbility) },
        { "LayOnHands", typeof(LayOnHandsAbility) },

        // Emil
        { "HexbladesCurse", typeof(HexbladesCurseAbility) },
        { "EldritchBlast", typeof(EldritchBlastAbility) },
        { "AccursedSpectre", typeof(AccursedSpectreAbility) },

        // Taran
        { "MirrorImage", typeof(MirrorImageAbility) },
        { "LunarVolley", typeof(LunarVolleyAbility) },
        { "MoonsongsSurge", typeof(MoonsongsSurgeAbility) },

        // Arle
        { "SparkheraldEquipment", typeof(SparkheraldEquipmentAbility) },
        { "DeployTurret", typeof(DeployTurretAbility) },
        { "MajorUpgrade", typeof(MajorUpgradeAbility) },

        // Issok
        { "HealingWords", typeof(HealingWordsAbility) },
        { "IterativeEnhancement", typeof(IterativeEnhancementAbility) },
        { "CallForAid", typeof(CallForAidAbility) },

        // Wrenn
        { "Dash", typeof(DashAbility) },
        { "SecondWind", typeof(SecondWindAbility) },
        { "Amy", typeof(AmyAbility) },
        { "TragicBackstory", typeof(TragicBackstoryAbility) },

        // Smolder
        { "FlamingCompanion", typeof(FlamingCompanionAbility) },
        { "BurnTogetherSmolder", typeof(BurnTogetherSmolderAbility) },
        { "FlameStrike", typeof(FlameStrikeAbility) },

        // Buzz
        { "BurnTogetherBuzz", typeof(BurnTogetherBuzzAbility) },
        { "BlazingReincarnation", typeof(BlazingReincarnationAbility) },
        { "FireBolt", typeof(FireBoltAbility) },

        // Mitchell Katan
        { "MightOfTheDead", typeof(MightOfTheDeadAbility) },
        { "LifespunArtifact", typeof(LifespunArtifactAbility) },
        { "DeathspunArtifact", typeof(DeathspunArtifactAbility) },
        { "ArtifactSummonHound", typeof(ArtifactSummonHoundAbility) },
        { "ArtifactLifeBuff", typeof(ArtifactLifeBuffAbility) },

        // Metha
        { "Agriculture", typeof(AgricultureAbility) },
        { "Horticulture", typeof(HorticultureAbility) },
        { "BotanistsFury", typeof(BotanistsFuryAbility) },

        // Guthren
        { "ResearchTheLost", typeof(ResearchTheLostAbility) },
        { "SageAdvice", typeof(SageAdviceAbility) },
        { "BestOfABadThing", typeof(BestOfABadThingAbility) },

        // CAMPAIGN 1
        // Kthellan
        { "TridentsSpeed", typeof(TridentsSpeedAbility) },
        { "WrathOfTheStorm", typeof(WrathOfTheStormAbility) },
        { "LightningsFlash", typeof(LightningsFlashAbility) },

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
