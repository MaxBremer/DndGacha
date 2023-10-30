using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelingCurve", menuName = "DndGacha/LevelingCurve", order = 1)]
public class ScriptableLevelingCurve : ScriptableObject
{
    public LevelingCurveType LevelCurveType;

    public LevelData[] levels = new LevelData[19];

    [System.Serializable]
    public class LevelData
    {
        public int attack;
        public int health;
        public int speed;
        public int initiative;
    }
}
