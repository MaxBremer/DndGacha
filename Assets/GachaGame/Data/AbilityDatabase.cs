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

        // Wilfina
        { "Towerlocked", typeof(TowerlockedAbility) },
        { "Towerwarp", typeof(TowerwarpAbility) },
        { "MedivhsDisciples", typeof(MedivhsDisciplesAbility) },
        { "DustGraftedBody", typeof(DustGraftedBodyAbility) },

        // Alexander
        { "PaleReincarnation", typeof(PaleReincarnationAbility) },
        { "DeathAffinity", typeof(DeathAffinityAbility) },
        { "MrSandman", typeof(MrSandmanAbility) },

        // Dobondell
        { "PayUp", typeof(PayUpAbility) },
        { "UnpredictableRage", typeof(UnpredictableRageAbility) },
        { "UncannyResillience", typeof(UncannyResilienceAbility) },

        // Quillam
        { "Twinshot", typeof(TwinshotAbility) },
        { "Pistol", typeof(PistolAbility) },
        { "Assassinate", typeof(AssassinateAbility) },

        // Wallace
        { "InnerCitySafehouses", typeof(InnerCitySafehousesAbility) },
        { "SharedResources", typeof(SharedResourcesAbility) },
        { "SurpriseAssisstance", typeof(SurpriseAssistanceAbility) },
        { "MightAsWellBeDead", typeof(MightAsWellBeDeadAbility) },

        // Vamear Spen
        { "Archenemy", typeof(ArchenemyAbility) },
        { "MightOfTheFamily", typeof(MightOfTheFamilyAbility) },
        { "TheTiger", typeof(TheTigerAbility) },

        // Leah Knockwraith
        { "SundialVisions", typeof(SundialVisionsAbility) },
        { "StrategicSacrifice", typeof(StrategicSacrificeAbility) },
        { "AGlassStatue", typeof(AGlassStatueAbility) },

        // CAMPAIGN 1
        // Kthellan
        { "TridentsSpeed", typeof(TridentsSpeedAbility) },
        { "WrathOfTheStorm", typeof(WrathOfTheStormAbility) },
        { "LightningsFlash", typeof(LightningsFlashAbility) },

        // JOE CAMPAIGN
        // Bilbis
        { "Dirtshaper", typeof(DirtshaperAbility) },
        { "HoldPerson", typeof(HoldPersonAbility) },
        { "CallLightning", typeof(CallLightningAbility) },

        // Hiro
        { "FubikianLongbow", typeof(FubikianLongbowAbility) },
        { "SeizingOpportunity", typeof(SeizingOpportunityAbility) },
        { "RacistDogma", typeof(RacistDogmaAbility) },

        // Poe
        { "SeverFromReality", typeof(SeverFromRealityAbility) },
        { "Banish", typeof(BanishAbility) },
        { "AspectOfDust", typeof(AspectOfDustAbility) },

        // Burk
        { "StandingLeap", typeof(StandingLeapAbility) },
        { "TongueOfWarriors", typeof(TongueOfWarriorsAbility) },
        { "OcarinaPlayer", typeof(OcarinaPlayerAbility) },

        // Albert Foxe
        { "KnowThyEnemy", typeof(KnowThyEnemyAbility) },
        { "KnowThyself", typeof(KnowThyselfAbility) },
        { "EasilyFrightened", typeof(EasilyFrightenedAbility) },

        // Stone Golem
        { "GentleGiant", typeof(GentleGiantAbility) },
        { "StonySkin", typeof(StonySkinAbility) },
        { "CrushingStep", typeof(CrushingStepAbility) },

        // Obsidian Goldm
        { "Fragile", typeof(FragileAbility) },
        { "ObsidianSkin", typeof(ObsidianSkinAbility) },

        // Alan
        { "LearningMagic", typeof(LearningMagicAbility) },
        { "LightningOfTheBeholder", typeof(LightningOfTheBeholderAbility) },
        { "NobleFriend", typeof(NobleFriendAbility) },

        // James Hunter:
        { "SparkstoneRifle", typeof(SparkstoneRifleAbility) },
        { "Resourceful", typeof(ResourcefulAbility) },

        // Overseer
        { "HarshLeadership", typeof(HarshLeadershipAbility) },
        { "StrictOversight", typeof(StrictOversightAbility) },
        { "DualDaggers", typeof(DualDaggersAbility) },

        // The Eye
        { "Methodical", typeof(MethodicalAbility) },
        { "AuraOfNightmares", typeof(AuraOfNightmaresAbility) },
        { "Enthrall", typeof(EnthrallAbility) },

        // The Worldwatchers Idol
        { "SkinOfEyes", typeof(SkinOfEyesAbility) },
        { "LuringPupils", typeof(LuringPupilsAbility) },
        { "RealityIsMine", typeof(RealityIsMineAbility) },

        // Agmal
        { "LimbOfChains", typeof(LimbOfChainsAbility) },
        { "LetsMakeADeal", typeof(LetsMakeADealAbility) },
        { "FoldingReality", typeof(FoldingRealityAbility) },

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
