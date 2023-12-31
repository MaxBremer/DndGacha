using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private const int POINTS_TO_WIN = 6;

    private const int POINT_GAIN_PRIORITY = 5;

    private GameArgs _myArgs;

    public Game(GameArgs args)
    {
        _myArgs = args;
    }

    public Player[] Players;

    public int CurrentPlayerIndex;

    public bool GameOngoing = true;

    public Player CurrentPlayer => Players[CurrentPlayerIndex];

    public int TurnCount;

    public GachaGrid GameGrid;

    public List<Creature> AllCreatures = new List<Creature>();

    public int GridWidth;
    public int GridHeight;

    public bool IsInitTurn => TurnCount % 2 == 1 ;

    public int CurrentInitiative = 0;

    public void Init(bool initPlayers = true)
    {
        if (_myArgs == null)
        {
            Debug.LogError("GAME INIT ATTEMPTED WITHOUT DEFINED GAME ARGS");
            return;
        }

        // Initialize Grid
        GameGrid = new GachaGrid(_myArgs.GridXSize, _myArgs.GridYSize);
        GridWidth = _myArgs.GridXSize;
        GridHeight = _myArgs.GridYSize;

        // Initialize Players
        if (initPlayers)
        {
            InitPlayers();
        }

        // First players turn.
        CurrentPlayerIndex = 0;

        // First Turn
        TurnCount = 1;

        // RIGHT NOW, IT SHOULD ALWAYS BE 2 PLAYERS AND AT LEAST 4 XWIDTH FOR GRID
        if (_myArgs.Players.Length == 2 && _myArgs.GridXSize >= 4)
        {
            Setup2PCallSpots();
        }
        else
        {
            Debug.LogError("ERROR: Initialized non-2player game OR grid was too small.");
        }

        EventManager.InitializeGame(this);
        EventManager.StartListening(GachaEventType.CreatureDies, CreatureDied_GainPointIfPrime, POINT_GAIN_PRIORITY);

        ChoiceManager.Reset(this);

        GameEventLog.Initialize(this);

        StartOfTurnSetup();
    }

    public GameArgs MyGameArgs { get => _myArgs; }

    public void CallCharacter(Creature called, GridSpace callLocation, Player caller)
    {
        caller.CreatureCalled(called);
        SummonCreature(called, callLocation);
        called.IsPrime = true;
        EventManager.Invoke(GachaEventType.CreatureCalled, caller, new CreatureSummonArgs() { BeingSummoned = called, LocationOfSummon = callLocation });
    }

    public void SummonCreature(Creature summoned, GridSpace location)
    {
        if(location.isBlocked)
        {
            Debug.LogError("ERROR: Attempted summon to occupied or blocked space. Should not occur.");
            return;
        }

        GameGrid.CreatureEntersSpace(summoned, location);
        summoned.Summoned();
        if(summoned.Controller != null)
        {
            summoned.Controller.CreatureSummoned(summoned);
        }

        if (!AllCreatures.Contains(summoned))
        {
            AllCreatures.Add(summoned);
        }

        var summonArgs = new CreatureSummonArgs() { BeingSummoned = summoned, LocationOfSummon = location };
        EventManager.Invoke(GachaEventType.CreatureSummoned, this, summonArgs);
    }

    public void RemoveCreature(Creature c)
    {
        if(c == null || !AllCreatures.Contains(c))
        {
            return;
        }

        c.Controller.CreatureRemoved(c);
        GridSpace gs = null;
        if (c.IsOnBoard)
        {
            gs = c.MySpace;
            GameGrid.CreatureLeavesSpace(c);
            c.LeaveBoard();
        }

        AllCreatures.Remove(c);

        EventManager.Invoke(GachaEventType.CreatureRemoved, this, new CreatureDiesArgs() { CreatureDied = c, WhereItDied = gs });
        c.RemoveAllTriggers();
    }

    public void GainPoint(Player p)
    {
        p.Points++;
        EventManager.Invoke(GachaEventType.PointGained, this, new PointGainedArgs() { GainedPoint = p });
        if (p.Points >= POINTS_TO_WIN)
        {
            Win(p);
        }
    }

    public void GainPoint(List<Player> players)
    {
        foreach (var p in players)
        {
            GainPoint(p);
            if (!GameOngoing)
            {
                // TODO: Change this to allow potential draws?
                break;
            }
        }
    }

    public void Win(Player p)
    {
        EventManager.Invoke(GachaEventType.GameOver, this, new GameOverArgs() { PlayerWhoWon = p });
        // Game over shenanigans.
        var playerNum = p.MyPlayerIndex + 1;
        Debug.Log("Game Over! Player " + playerNum + " won!");
        GameOngoing = false;
    }

    public void EndTurn()
    {
        var EOTArgs = new TurnEndArgs() { PlayerWhoseTurnIsEnding = CurrentPlayerIndex };
        EndOfTurnSetup();
        EventManager.Invoke(GachaEventType.EndOfTurn, this, EOTArgs);
        // If it's not the last players turn, cycle to next player. Otherwise, next REAL turn.
        if (CurrentPlayerIndex >= Players.Length - 1)
        {
            CurrentPlayerIndex = 0;
            TurnCount++;
        }
        else
        {
            CurrentPlayerIndex++;
        }

        StartOfTurnSetup();
    }

    public List<Player> GetOpponents(Player p)
    {
        var retList = new List<Player>();
        foreach (var player in Players)
        {
            if(player != p)
            {
                retList.Add(player);
            }
        }

        return retList;
    }

    public void PrintEventText(string eventText)
    {
        Debug.Log(eventText);
    }

    private void EndOfTurnSetup()
    {
        foreach (var creature in Players[CurrentPlayerIndex].OnBoardCreatures)
        {
            creature.EndOfTurn();
        }
    }

    private void StartOfTurnSetup()
    {
        foreach (var creature in Players[CurrentPlayerIndex].OnBoardCreatures)
        {
            creature.StartOfTurnRefresh();
        }
        if (IsInitTurn)
        {
            Players[CurrentPlayerIndex].CanCallThisTurn = true;
            //Players[CurrentPlayerIndex].NumCallsThisTurn += 1;
            CurrentInitiative = ((TurnCount - 1) / 2) + 1;
        }

        var SOTArgs = new TurnStartArgs() { PlayerWhoseTurnIsStarting = CurrentPlayerIndex };
        EventManager.Invoke(GachaEventType.StartOfTurn, this, SOTArgs);
    }

    private void InitPlayers()
    {
        var pList = new List<Player>();
        // For tracking player index while they're initialized.
        var count = 0;
        foreach (var pArgs in _myArgs.Players)
        {
            var Player = new Player(new PlayerArgs() { type = pArgs.type }) { MyPlayerIndex = count, };
            //Player.Creatures.AddRange(pArgs.startingCreatures);
            Player.MyGame = this;
            foreach (var creature in pArgs.startingCreatures)
            {
                creature.Controller = Player;
                Player.PutInReserve(creature);
                AllCreatures.Add(creature);
            }
            pList.Add(Player);
            count++;
        }
        Players = pList.ToArray();
    }

    private void Setup2PCallSpots()
    {
        // First player is on the near side, second far side.
        int midPoint = (GridWidth + 1) / 2;
        if(GridWidth >= 4)
        {
            int[] options = GridWidth % 2 == 0 ? new int[] { midPoint - 2, midPoint - 1, midPoint, midPoint + 1 } : new int[] { midPoint - 2, midPoint - 1, midPoint };
            foreach (int xPos in options)
            {
                Players[0].ValidInitSpaces.Add(GameGrid[(xPos, 0)]);
                Players[1].ValidInitSpaces.Add(GameGrid[(xPos, GridHeight - 1)]);
            }
        }
        else
        {
            Players[0].ValidInitSpaces.Add(GameGrid[(midPoint - 1, 0)]);
            Players[1].ValidInitSpaces.Add(GameGrid[(midPoint - 1, GridHeight - 1)]);
        }
    }

    private void CreatureDied_GainPointIfPrime(object sender, EventArgs e)
    {
        if (sender is Creature creat && creat.IsPrime)
        {
            // ALL PLAYERS NOT CONTROLLING THAT CREATURE GET A POINT
            foreach (var player in Players)
            {
                if(player != creat.Controller)
                {
                    GainPoint(player);
                }
            }
        }
    }
}

public class GameArgs
{
    public int GridXSize;
    public int GridYSize;
    public PlayerArgs[] Players;
}