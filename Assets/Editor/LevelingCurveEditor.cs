using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScriptableLevelingCurve))]
public class LevelingCurveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Calculate and display the totals
        ScriptableLevelingCurve curve = (ScriptableLevelingCurve)target;

        int totalAttack = 0;
        int totalHealth = 0;
        int totalSpeed = 0;
        int totalInitiative = 0;

        foreach (var level in curve.levels)
        {
            totalAttack += level.attack;
            totalHealth += level.health;
            totalSpeed += level.speed;
            totalInitiative += level.initiative;
        }

        // Display the totals
        EditorGUILayout.LabelField("Total Attack", totalAttack.ToString());
        EditorGUILayout.LabelField("Total Health", totalHealth.ToString());
        EditorGUILayout.LabelField("Total Speed", totalSpeed.ToString());
        EditorGUILayout.LabelField("Total Initiative", totalInitiative.ToString());

        // Draw a separator
        EditorGUILayout.Space();

        // Draw the default inspector below the totals
        DrawDefaultInspector();
    }
}
