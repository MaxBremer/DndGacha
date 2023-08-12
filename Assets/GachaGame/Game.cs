using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private const int POINTS_TO_WIN = 6;

    private GameArgs _myArgs;

    public Game(GameArgs args)
    {
        _myArgs = args;
    }

    public Player[] Players;

    public int CurrentPlayerIndex;

    public Player CurrentPlayer => Players[CurrentPlayerIndex];

    public int TurnCount;

    public GachaGrid GameGrid;

    public List<Creature> AllCreatures = new List<Creature>();

    public int GridWidth;
    public int GridHeight;

    public bool IsInitTurn => TurnCount % 2 == 1 ;

    public int CurrentInitiative = 0;

    public void Init()
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
        InitPlayers();

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
        EventManager.StartListening("CreatureDies", CreatureDied_GainPointIfPrime);

        ChoiceManager.Reset(this);

        StartOfTurnSetup();
    }

    /*public void CallCharacter(CreatureGameBase calledBase, GridSpace callLoc, Player caller)
    {
        var creat = new Creature();
        creat.InitFromBase(calledBase);
        CallCharacter(creat, callLoc, caller);
    }*/

    public void CallCharacter(Creature called, GridSpace callLocation, Player caller)
    {
        // EVENTUALLY, will have to decision-make GridSpace out of possible locations.
        // Although maybe just write separate function? That can call this one on return?
        SummonCreature(called, callLocation);
        caller.CreatureCalled(called);
        called.IsPrime = true;
        EventManager.Invoke("CreatureCalled", caller, new CreatureSummonArgs() { BeingSummoned = called, LocationOfSummon = callLocation });
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

        if (!AllCreatures.Contains(summoned))
        {
            AllCreatures.Add(summoned);
        }

        var summonArgs = new CreatureSummonArgs() { BeingSummoned = summoned, LocationOfSummon = location };
        EventManager.Invoke("CreatureSummoned", this, summonArgs);
    }

    public void GainPoint(Player p)
    {
        p.Points++;
        EventManager.Invoke("PointGained", this, new PointGainedArgs() { GainedPoint = p });
        if (p.Points >= POINTS_TO_WIN)
        {
            Win(p);
        }
    }

    public void Win(Player p)
    {
        EventManager.Invoke("GameOver", this, new GameOverArgs() { PlayerWhoWon = p });
        // Game over shenanigans.
    }

    public void EndTurn()
    {
        var EOTArgs = new TurnEndArgs() { PlayerWhoseTurnIsEnding = CurrentPlayerIndex };
        EndOfTurnSetup();
        EventManager.Invoke("EndOfTurn", this, EOTArgs);
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

        var SOTArgs = new TurnStartArgs() { PlayerWhoseTurnIsStarting = CurrentPlayerIndex };
        StartOfTurnSetup();
        EventManager.Invoke("StartOfTurn", this, SOTArgs);
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
            CurrentInitiative = ((TurnCount - 1) / 2) + 1;
        }
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