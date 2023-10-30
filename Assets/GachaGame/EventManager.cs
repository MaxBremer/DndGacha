using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EventManager
{
    private const int LAYER_LIMIT = 500;

    // So for priorities:
    // LOWER priority means it will be called BEFORE other calls.
    // HIGHER priority means it will be called AFTER other calls.
    // For example: passive that negates damage in certain scenarios should have HIGH priority, meaning it negates any other dmg modifiers that came before.
    // passive that only multiplies base damage of an attack should have LOW priority, meaning it will trigger before other stat changes are made.
    // 0 is default priority.
    private const int MIN_PRIORITY = -5;
    private const int MAX_PRIORITY = 5;

    private class PriorityEventHandler
    {
        public EventHandler Handler { get; set; }
        public int Priority { get; set; }

        public PriorityEventHandler(EventHandler handler, int priority)
        {
            Handler = handler;
            Priority = priority;
        }
    }

    //private static Dictionary<GachaEventType, EventHandler> _basePriorityEvents;
    //private static Dictionary<GachaEventType, List<PriorityEventHandler>> _priorityEvents;
    private static Dictionary<GachaEventType, Dictionary<int, EventHandler>> _priorityEvents;

    private static Dictionary<GachaEventType, int> _layersDeep;
    private static Dictionary<GachaEventType, List<(int, EventHandler)>> _listenersToBeRemoved;
    private static Dictionary<GachaEventType, List<(int, EventHandler)>> _listenersToBeAdded; 

    public static Game CurrentGame;

    public static int TotalInvokeLayers = 0;

    public static void Reset()
    {
        _layersDeep ??= new Dictionary<GachaEventType, int>();
        _layersDeep.Clear();

        /*if (_basePriorityEvents == null)
        {
            _basePriorityEvents = new Dictionary<GachaEventType, EventHandler>();
        }

        List<(GachaEventType, EventHandler)> toDelete = new List<(GachaEventType, EventHandler)>();
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

        _basePriorityEvents.Clear();*/

        _priorityEvents ??= new Dictionary<GachaEventType, Dictionary<int, EventHandler>>();// Dictionary<GachaEventType, List<PriorityEventHandler>>();
        _priorityEvents.Clear();

        _listenersToBeRemoved ??= new Dictionary<GachaEventType, List<(int, EventHandler)>>();
        _listenersToBeRemoved.Clear();

        _listenersToBeAdded ??= new Dictionary<GachaEventType, List<(int, EventHandler)>>();
        _listenersToBeAdded.Clear();

        CurrentGame = null;
    }

    public static void InitializeGame(Game g)
    {
        CurrentGame = g;
        Reset();
    }

    public static void StartListening(GachaEventType eventName, EventHandler listener, int priority)
    {
        /*if (_basePriorityEvents.TryGetValue(eventName, out EventHandler eventH))
        {
            eventH += listener;
            _basePriorityEvents[eventName] = eventH;
        }
        else
        {
            eventH += listener;
            _basePriorityEvents.Add(eventName, eventH);
            _layersDeep.Add(eventName, 0);
        }*/

        if (priority < MIN_PRIORITY || priority > MAX_PRIORITY)
            throw new ArgumentOutOfRangeException(nameof(priority), $"Priority must be between {MIN_PRIORITY} and {MAX_PRIORITY}");

        if (!_priorityEvents.TryGetValue(eventName, out var handlers))
        {
            //handlers = new List<PriorityEventHandler>();
            handlers = new Dictionary<int, EventHandler>();
            handlers.Add(priority, listener);
            _priorityEvents.Add(eventName, handlers);
            _layersDeep[eventName] = 0;
        }
        else
        {
            if(_layersDeep[eventName] > 0) //TotalInvokeLayers > 0)
            {
                if(!_listenersToBeAdded.TryGetValue(eventName, out var handlersToAdd))
                {
                    handlersToAdd = new List<(int, EventHandler)>();
                    handlersToAdd.Add((priority, listener));
                    _listenersToBeAdded.Add(eventName, handlersToAdd);
                }
                else
                {
                    handlersToAdd.Add((priority, listener));
                }
            }
            else
            {
                /*if (!handlers.TryGetValue(priority, out var handler))
                {
                    handlers.Add(priority, listener);
                }
                else
                {
                    handler += listener;
                    handlers[priority] = handler;
                }*/
                AddListener(eventName, listener, priority);
            }
        }

        /*var newHandler = new PriorityEventHandler(listener, priority);
        int index = handlers.FindIndex(h => h.Priority > priority);
        if (index == -1)
            handlers.Add(newHandler);
        else
            handlers.Insert(index, newHandler);*/
    }

    private static void AddListener(GachaEventType eventName, EventHandler listener, int priority)
    {
        var handlers = _priorityEvents[eventName];

        if (!handlers.TryGetValue(priority, out var handler))
        {
            handlers.Add(priority, listener);
        }
        else
        {
            handler += listener;
            handlers[priority] = handler;
        }
    }

    public static void StopListening(GachaEventType eventName, EventHandler listener, int priority)
    {
        /*if (_basePriorityEvents.TryGetValue(eventName, out EventHandler eventT))
        {
            eventT -= listener;
            _basePriorityEvents[eventName] = eventT;
        }*/

        if (_priorityEvents.TryGetValue(eventName, out var handlers))
        {
            if (_layersDeep[eventName] > 0) //TotalInvokeLayers > 0)
            {
                if (!_listenersToBeRemoved.TryGetValue(eventName, out var toBeRemoved))
                {
                    toBeRemoved = new List<(int, EventHandler)>();
                    _listenersToBeRemoved[eventName] = toBeRemoved;
                }
                toBeRemoved.Add((priority, listener));
            }
            else
            {
                /*var itemToRemove = handlers.FirstOrDefault(h => h.Value == listener);
                if (!itemToRemove.Equals(default(KeyValuePair<int, EventHandler>)))
                    handlers.Remove(itemToRemove.Key);*/
                if(handlers.TryGetValue(priority, out var eventHandler))
                {
                    eventHandler -= listener;
                    handlers[priority] = eventHandler;
                }
            }
        }
    }

    public static void Invoke(GachaEventType eventName, object cause, EventArgs args)
    {
        /*if (!_basePriorityEvents.ContainsKey(eventName))
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
        }*/

        if (!_priorityEvents.ContainsKey(eventName))
        {
            //Debug.LogWarning("WARNING: Invoked event '" + eventName + "' that either has no listeners or was not initialized correctly");
            return;
        }
        if (_layersDeep[eventName] < LAYER_LIMIT)
        {
            TotalInvokeLayers++;
            _layersDeep[eventName]++;
            foreach (var handler in _priorityEvents[eventName])
            {
                //Debug.Log("Doing one invoke for " + eventName.ToString());
                handler.Value?.Invoke(cause, args);
                //Debug.Log("Done with that one for " + eventName.ToString());
            }
            _layersDeep[eventName]--;
            TotalInvokeLayers--;

            // Process the to-be-removed list
            if (_listenersToBeRemoved.TryGetValue(eventName, out var toBeRemoved) && toBeRemoved.Count > 0 && _layersDeep[eventName] < 1)
            {
                foreach (var listener in toBeRemoved)
                {
                    /*var itemToRemove = _priorityEvents[eventName].FirstOrDefault(h => h.Handler == listener);
                    if (itemToRemove != null)
                        _priorityEvents[eventName].Remove(itemToRemove);*/
                    if(_priorityEvents.TryGetValue(eventName, out var handlers))
                    {
                        if (handlers.TryGetValue(listener.Item1, out var eventHandler))
                        {
                            eventHandler -= listener.Item2;
                            handlers[listener.Item1] = eventHandler;
                        }
                    }
                }
                toBeRemoved.Clear();
            }

            if(_listenersToBeAdded.TryGetValue(eventName, out var toBeAdded) && toBeAdded.Count > 0 && _layersDeep[eventName] < 1)
            {
                foreach (var listener in toBeAdded)
                {
                    AddListener(eventName, listener.Item2, listener.Item1);
                }
                toBeAdded.Clear();
            }
        }
        else
        {
            Debug.LogWarning("WARNING: Reached invoke limit for event '" + eventName + "'.");
        }
    }
}