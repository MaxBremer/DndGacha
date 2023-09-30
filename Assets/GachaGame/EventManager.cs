using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    private const int LAYER_LIMIT = 100;

    // So for priorities:
    // LOWER priority means it will be called BEFORE other calls.
    // HIGHER priority means it will be called AFTER other calls.
    // For example: passive that negates damage in certain scenarios should have HIGH priority, meaning it negates any other dmg modifiers that came before.
    // passive that only multiplies base damage of an attack should have LOW priority, meaning it will trigger before other stat changes are made.
    // 0 is default priority.
    private const int MIN_PRIORITY = -5;
    private const int MAX_PRIORITY = 5;

    private static Dictionary<string, EventHandler> _basePriorityEvents;
    private static Dictionary<string, int> _layersDeep;

    public static Game CurrentGame;

    public static int TotalInvokeLayers = 0;

    public static void Reset()
    {
        if(_layersDeep == null)
        {
            _layersDeep = new Dictionary<string, int>();
        }
        else
        {
            _layersDeep.Clear();
        }

        if (_basePriorityEvents == null)
        {
            _basePriorityEvents = new Dictionary<string, EventHandler>();
        }

        List<(string, EventHandler)> toDelete = new List<(string, EventHandler)>();
        foreach (var key in _basePriorityEvents.Keys)
        {
            var invList = _basePriorityEvents[key].GetInvocationList();
            foreach (EventHandler item in invList)
            {
                //_events[key] -= (EventHandler)item;
                toDelete.Add((key, item));
            }
        }

        foreach (var item in toDelete)
        {
            _basePriorityEvents[item.Item1] -= item.Item2;
        }

        _basePriorityEvents.Clear();

        CurrentGame = null;
    }

    public static void InitializeGame(Game g)
    {
        CurrentGame = g;
        Reset();
    }

    public static void StartListening(string eventName, EventHandler listener)
    {
        if (_basePriorityEvents.TryGetValue(eventName, out EventHandler eventH))
        {
            eventH += listener;
            _basePriorityEvents[eventName] = eventH;
        } else
        {
            eventH += listener;
            _basePriorityEvents.Add(eventName, eventH);
            _layersDeep.Add(eventName, 0);
        }
    }

    public static void StopListening(string eventName, EventHandler listener)
    {
        if (_basePriorityEvents.TryGetValue(eventName, out EventHandler eventT))
        {
            eventT -= listener;
            _basePriorityEvents[eventName] = eventT;
        }
    }

    public static void Invoke(string eventName, object cause, EventArgs args)
    {
        if (!_basePriorityEvents.ContainsKey(eventName))
        {
            //Debug.LogWarning("WARNING: Invoked event '" + eventName + "' that either has no listeners or was not initialized correctly");
            return;
        }
        if (_layersDeep[eventName] < LAYER_LIMIT)
        {
            TotalInvokeLayers++;
            _layersDeep[eventName]++;
            _basePriorityEvents[eventName]?.Invoke(cause, args);
            _layersDeep[eventName]--;
            TotalInvokeLayers--;
        }
        else
        {
            Debug.LogWarning("WARNING: Reached invoke limit for event '" + eventName + "'.");
        }
    }
}

// So the issue behind switching to enum is many custom events. Often abilities require a custom invoke/listen for their own special purposes.
public enum GachaEventType
{
    StartOfTurn,
    EndOfTurn,

    BeforeAttack,
    AfterAttack,

    BeforeDamage,
    AfterDamage,

    BeforeCreatureStatsChange,
    AfterCreatureStatsChange,

    BeforeAbilityTrigger,
    AfterAbilityTrigger,

    CreatureLeavesSpace,
    CreatureEntersSpace,
    CreatureMovesThroughSpace,

    CreatureDies,
    PointGained,
    GameOver,

    CreatureLostAbility,
    CreatureGainedAbility,

    CreatureCalled,
    CreatureSummoned,

    CreatureReserved,
    CreatureLeavesReserve,

    CustomEvent,
}
