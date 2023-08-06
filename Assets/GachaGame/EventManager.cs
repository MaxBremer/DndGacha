using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    private const int LAYER_LIMIT = 100;

    private static Dictionary<string, EventHandler> _events;
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

        if (_events == null)
        {
            _events = new Dictionary<string, EventHandler>();
        }

        List<(string, EventHandler)> toDelete = new List<(string, EventHandler)>();
        foreach (var key in _events.Keys)
        {
            var invList = _events[key].GetInvocationList();
            foreach (EventHandler item in invList)
            {
                //_events[key] -= (EventHandler)item;
                toDelete.Add((key, item));
            }
        }

        foreach (var item in toDelete)
        {
            _events[item.Item1] -= item.Item2;
        }

        _events.Clear();

        CurrentGame = null;
    }

    public static void InitializeGame(Game g)
    {
        CurrentGame = g;
        Reset();
    }

    public static void StartListening(string eventName, EventHandler listener)
    {
        if (_events.TryGetValue(eventName, out EventHandler eventH))
        {
            eventH += listener;
            _events[eventName] = eventH;
        } else
        {
            eventH += listener;
            _events.Add(eventName, eventH);
            _layersDeep.Add(eventName, 0);
        }
    }

    public static void StopListening(string eventName, EventHandler listener)
    {
        if (_events.TryGetValue(eventName, out EventHandler eventT))
        {
            eventT -= listener;
            _events[eventName] = eventT;
        }
    }

    public static void Invoke(string eventName, object cause, EventArgs args)
    {
        if (!_events.ContainsKey(eventName))
        {
            //Debug.LogWarning("WARNING: Invoked event '" + eventName + "' that either has no listeners or was not initialized correctly");
            return;
        }
        if (_layersDeep[eventName] < LAYER_LIMIT)
        {
            TotalInvokeLayers++;
            _layersDeep[eventName]++;
            _events[eventName]?.Invoke(cause, args);
            _layersDeep[eventName]--;
            TotalInvokeLayers--;
        }
        else
        {
            Debug.LogWarning("WARNING: Reached invoke limit for event '" + eventName + "'.");
        }
    }
}
