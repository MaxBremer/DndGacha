using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class LevelingCurveManager
{
    private static bool isInitialized = false;
    private static readonly Dictionary<LevelingCurveType, ScriptableLevelingCurve> curves = new Dictionary<LevelingCurveType, ScriptableLevelingCurve>();

    public static void InitializeLevelCurves()
    {
        if (isInitialized) return;

        // Assuming your ScriptableLevelingCurves are in a folder named "LevelingCurves" inside a "Resources" folder.
        ScriptableLevelingCurve[] curves = Resources.LoadAll<ScriptableLevelingCurve>("LevelCurves");

        foreach (var curve in curves)
        {
            RegisterLevelingCurve(curve);
        }

        isInitialized = true;
    }

    public readonly static int[] XPAmountsRequiredForLevelUp = new int[]
    {
        300,
        900,
        2700,
        6500,
        14000,
        23000,
        34000,
        48000,
        64000,
        85000,
        100000,
        120000,
        140000,
        165000,
        195000,
        225000,
        265000,
        305000,
        355000,
    };

    /*public static void InitializeLevelCurves()
    {
        Addressables.LoadAssetsAsync<ScriptableLevelingCurve>("LevelCurves", null).Completed += OnLevelCurvesLoaded;
    }

    private static void OnLevelCurvesLoaded(AsyncOperationHandle<IList<ScriptableLevelingCurve>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<ScriptableLevelingCurve> levelCurves = handle.Result;

            foreach (var curve in levelCurves)
            {
                curves[curve.LevelCurveType] = curve;
            }
            isInitialized = true;
        }
        else
        {
            Debug.LogError("Failed to load level curves.");
        }
    }*/

    // Call this method at the start of your game or during the loading phase to populate the dictionary.
    public static void RegisterLevelingCurve(ScriptableLevelingCurve curve)
    {
        if (curve == null)
        {
            Debug.LogError("Attempted to register a null ScriptableLevelingCurve.");
            return;
        }

        if (curves.ContainsKey(curve.LevelCurveType))
        {
            Debug.LogError($"A curve with type {curve.LevelCurveType} is already registered.");
        }
        else
        {
            curves[curve.LevelCurveType] = curve;
        }
    }

    // Call this method to retrieve a ScriptableLevelingCurve by its LevelingCurveType.
    public static ScriptableLevelingCurve GetLevelingCurve(LevelingCurveType type)
    {
        if (curves.TryGetValue(type, out ScriptableLevelingCurve curve))
        {
            return curve;
        }
        else
        {
            Debug.LogError($"No ScriptableLevelingCurve found for type {type}.");
            return null;
        }
    }
}
