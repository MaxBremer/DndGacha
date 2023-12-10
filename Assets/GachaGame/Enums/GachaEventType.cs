public enum GachaEventType
{
    StartOfTurn,
    EndOfTurn,

    AbilityCooldownLower,

    BeforeAttack,
    AfterAttack,

    BeforeDamage,
    AfterDamage,

    BeforeHealing,
    AfterHealing,

    BeforeCreatureDies,
    AfterCreatureDies,

    BeforeCreatureStatsChange,
    AfterCreatureStatsChange,

    BeforeAbilityTrigger,
    AfterAbilityTrigger,

    BeforeActiveAbilityActivates,
    AfterActiveAbilityActivates,

    LostAbility,
    GainedAbility,

    CreatureLeavesSpace,
    CreatureEntersSpace,
    CreatureMovesThroughSpace,

    CreatureDies,
    CreatureRemoved,
    PointGained,
    GameOver,

    CreatureLostAbility,
    CreatureGainedAbility,

    CreatureCalled,
    CreatureSummoned,

    CreatureActed,
    CreatureMoved,

    CreatureReserved,
    CreatureLeavesReserve,

    CreatureLeavesBoard,

    CreatureSelectingAttackTargets,
    CreatureMovesFound,
    CreatureAbilitySelectingTargets,
    BeforeGridSpaceMoveWeightGet,

    CustomEvent,
    TestTrigger,
    TestAddingTrigger,
    NULL
}
