using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class CSVImporter : Editor
{
    [MenuItem("Tools/Import Level Curves from CSV")]
    public static void ImportCSV()
    {
        string path = EditorUtility.OpenFilePanel("Select CSV", "", "csv");
        if (path.Length != 0)
        {
            string assetName = EditorUtility.SaveFilePanelInProject("Save Level Curve", "NewLevelCurve", "asset", "Please enter a file name to save the level curve to");
            if (string.IsNullOrEmpty(assetName))
            {
                // User cancelled or closed the save panel
                Debug.Log("Level Curve creation was cancelled by the user.");
                return;
            }

            var lines = File.ReadAllLines(path);

            List<LevelData> levelsToAdd = new List<LevelData>();
            foreach (var line in lines)
            {
                var data = line.Split(',');
                if(data[0] == "Level")
                {
                    continue;
                }

                // Assuming data follows the order: Level, Health, Attack, Speed, Initiative
                var level = int.Parse(data[0]);
                var health = int.Parse(data[3]);
                var attack = int.Parse(data[4]);
                var speed = int.Parse(data[1]);
                var initiative = int.Parse(data[2]);

                // Find existing or create new ScriptableObject for the level
                var levelData = new LevelData()
                {
                    attack = attack,
                    health = health,
                    speed = speed,
                    initiative = -1 * initiative,
                };

                levelsToAdd.Add(levelData);
            }

            ScriptableLevelingCurve curve = ScriptableObject.CreateInstance<ScriptableLevelingCurve>();
            curve.Initialize(levelsToAdd);

            AssetDatabase.CreateAsset(curve, assetName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Level Curve created at: " + assetName);
        }
    }
}