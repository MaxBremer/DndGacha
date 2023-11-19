using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelingCurve", menuName = "DndGacha/LevelingCurve", order = 1)]
public class ScriptableLevelingCurve : ScriptableObject
{
    public LevelingCurveType LevelCurveType;

    public LevelData[] levels = new LevelData[19];

    public void Initialize(List<LevelData> levelData)
    {
        for (int i = 0; i < 19; i++)
        {
            levels[i] = levelData[i];
        }
    }
}
