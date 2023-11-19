using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionCharacter
{
    private CreatureGameBase _creatureBase;
    private Rarity _rarity;
    private LevelData[] _levelCurveData = new LevelData[19];

    public CollectionCharacter(ScriptableCharacterBase creatBase)
    {
        _creatureBase = ScriptableCreatureConverter.GameBaseFromScriptableCharacter(creatBase);

        _rarity = creatBase.rarity;

        var levelCurve = LevelingCurveManager.GetLevelingCurve(creatBase.LevelCurveType);
        for (int i = 0; i < levelCurve.levels.Length; i++)
        {
            _levelCurveData[i] = new LevelData()
            {
                attack = levelCurve.levels[i].attack,
                health = levelCurve.levels[i].health,
                speed = levelCurve.levels[i].speed,
                initiative = levelCurve.levels[i].initiative,

            };
        }
    }

    public int Rank = 0;

    public int Level = 1;

    public int CurXP = 0;

    public int TotalXPRequiredForNextLevel = 0;

    public bool isReborn = false;

    public CreatureGameBase CreatureBase => _creatureBase;

    public int XPUntilNextLevel => TotalXPRequiredForNextLevel - CurXP;

    public CosmeticType Cosmetic = CosmeticType.NORMAL;

    public Rarity Rarity => _rarity;

    //Data type of skin?
    public int SkinIndex = 0;

    public int UnspentRankupPoints = 0;

    public int[] AbilityRanks = new int[] { 0, 0, 0, };

    public LevelData[] LevelCurveData => _levelCurveData;

    public void GainXP(int amount)
    {
        if (Level >=20)
        {
            return;
        }

        if (amount >= XPUntilNextLevel)
        {
            amount -= XPUntilNextLevel;
            LevelUp();
            GainXP(amount);
        }
        else
        {
            CurXP += amount;
        }
    }

    public void LevelUp()
    {
        if (Level < 20)
        {
            Level++;
            CurXP = 0;

            GainLevelData(LevelCurveData[Level - 2]);

            if (Level < 20)
            {
                TotalXPRequiredForNextLevel = LevelingCurveManager.XPAmountsRequiredForLevelUp[Level - 1];
            }
            else
            {
                TotalXPRequiredForNextLevel = 0;
            }
        }
    }

    public void RankUp()
    {
        if(Rank >= 3)
        {
            Debug.LogError("Attempted to Rank Up when already at Rank 3.");
            return;
        }

        Rank++;
        UnspentRankupPoints++;
    }

    public void RankUpAbil(int abilIndex)
    {
        if(AbilityRanks[abilIndex] >= 2)
        {
            Debug.LogError("Attempted to rank up fully ranked ability!");
            return;
        }

        if(UnspentRankupPoints == 0)
        {
            Debug.LogError("Attempted to rank up ability without rank up point.");
            return;
        }

        AbilityRanks[abilIndex]++;
        // TODO: ACTUALLY RANK UP THE ABILITY.
    }

    public bool CanAbilityRankUp(int abilIndex) => AbilityRanks[abilIndex] < 2;

    private void GainLevelData(LevelData data)
    {
        _creatureBase.Attack += data.attack;
        _creatureBase.Health += data.health;
        _creatureBase.Speed += data.speed;
        _creatureBase.Initiative = Math.Max(1, data.initiative + _creatureBase.Initiative);
    }
}
