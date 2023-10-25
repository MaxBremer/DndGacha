using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GameEventLog
{
    private static bool _listenersSetUp = false;
    private const int _myPriority = -5;
    private static Game _myGame;

    private static GachaEventType[] _typesToAddEventFor = new GachaEventType[]
    {
        GachaEventType.AfterAbilityTrigger,
    };

    public static List<TurnLog> FullLog = new List<TurnLog>();
    public static TurnLog CurrentTurnLog = null;
    public static List<GameEvent> CurrentPlayerTurnEvents = null;

    public static void Initialize(Game g)
    {
        Reset();

        _myGame = g;

        EventManager.StartListening(GachaEventType.StartOfTurn, StartOfTurn, _myPriority);
        foreach (var eventType in _typesToAddEventFor)
        {
            EventManager.StartListening(eventType, AddEvent, _myPriority);
        }

        _listenersSetUp = true;
    }

    public static void Reset()
    {
        FullLog.Clear();

        if (_listenersSetUp)
        {
            EventManager.StopListening(GachaEventType.StartOfTurn, StartOfTurn, _myPriority);

            foreach (var eventType in _typesToAddEventFor)
            {
                EventManager.StopListening(eventType, AddEvent, _myPriority);
            }

            _listenersSetUp = false;
        }
    }

    public static List<GameEvent> AllEvents 
    { 
        get
        {
            var returner = new List<GameEvent>();
            FullLog.ForEach(x => returner.AddRange(x.AllEventsThisTurn));
            return returner;
        } 
    }

    public static void AddEvent(object sender, EventArgs e)
    {
        GachaEventType eventType = DetermineEventType(sender, e);

        var eventToAdd = new GameEvent(eventType);

        switch (eventType)
        {
            case GachaEventType.StartOfTurn:
                eventToAdd.IntData.Add(_myGame.TurnCount);
                eventToAdd.IntData.Add((e as TurnStartArgs).PlayerWhoseTurnIsStarting);
                eventToAdd.IntData.Add(_myGame.CurrentInitiative);
                break;

            case GachaEventType.AfterAbilityTrigger:
                var abilArgs = e as AfterAbilityTriggersArgs;
                eventToAdd.StringData.Add(abilArgs.AbilThatTriggered.Name);
                break;
        }

        TrueAddEvent(eventToAdd);
    }

    public static TurnLog GetLogNTurnsAgo(int n)
    {
        var numToRemove = n + 1;
        if (FullLog.Count < numToRemove)
        {
            return null;
        }
        else
        {
            return FullLog[FullLog.Count() - numToRemove];
        }
    }

    private static void TrueAddEvent(GameEvent eventToAdd)
    {
        CurrentPlayerTurnEvents.Add(eventToAdd);
        _myGame.PrintEventText(eventToAdd.ToString());
    }

    private static void StartOfTurn(object sender, EventArgs e)
    {
        if(e is TurnStartArgs)
        {
            if (_myGame.CurrentPlayerIndex == 0)
            {
                var newTurnLog = new TurnLog() { TurnNumber = _myGame.TurnCount, IsInitiativeTurn = _myGame.IsInitTurn, InitiativeCount = _myGame.CurrentInitiative, };
                FullLog.Add(newTurnLog);
                CurrentTurnLog = newTurnLog;
            }

            List<GameEvent> CurPlayerTurnEventLog = new List<GameEvent>();
            CurrentPlayerTurnEvents = CurPlayerTurnEventLog;
            CurrentTurnLog.TurnEventsByPlayer.Add(CurPlayerTurnEventLog);
        }

        AddEvent(sender, e);
    }

    private static GachaEventType DetermineEventType(object sender, EventArgs e)
    {
        switch (e)
        {
            case TurnStartArgs _:
                return GachaEventType.StartOfTurn;

            case AfterAbilityTriggersArgs _:
                return GachaEventType.AfterAbilityTrigger;

            default:
                return GachaEventType.NULL;
        }
    }
}
