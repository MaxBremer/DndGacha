using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionCharacter
{
    private CreatureGameBase _creatureBase;

    public CollectionCharacter(CreatureGameBase creatBase)
    {
        _creatureBase = creatBase;
    }

    public int Rank = 0;

    public int Level = 1;

    public int CurXP = 0;

    public int TotalXPRequiredForNextLevel = 0;

    public int XPUntilNextLevel => TotalXPRequiredForNextLevel - CurXP;

    public CosmeticType Cosmetic = CosmeticType.NORMAL;

    //Data type of skin?
    public int SkinIndex = 0;

    public int UnspentRankupPoints = 0;

    public int[] AbilityRanks = new int[] { 0, 0, 0, };
}
