using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Player(PlayerArgs args)
    {
        Type = args.type;
    }

    public int MyPlayerIndex;

    public int Points = 0;

    //public bool CanCallThisTurn => NumCallsThisTurn >= 1;
    public bool CanCallThisTurn = false;

    public int NumCallsThisTurn = 0;

    public PlayerType Type;

    public List<Creature> OnBoardCreatures = new List<Creature>();

    public List<Creature> Graveyard = new List<Creature>();

    public List<Creature> Reserve = new List<Creature>();

    public List<GridSpace> ValidInitSpaces = new List<GridSpace>();

    public Game MyGame;

    public virtual void StartMakingChoices(Ability abil)
    {
        ChoiceManager.MakeChoicesRandomly(abil.ChoicesNeeded, abil);
        ChoiceManager.AbilityChoicesMade(abil);
    }

    public virtual void CreatureCalled(Creature creat)
    {
        if (Reserve.Contains(creat))
        {
            RemoveFromReserve(creat);
            OnBoardCreatures.Add(creat);
            CanCallThisTurn = false;
            //NumCallsThisTurn -= 1;
        }
        else
        {
            Debug.LogWarning("WARNING: Player tried to call creature that was not in reserve.");
        }
    }

    public virtual void CreatureSummoned(Creature creat)
    {
        if (!OnBoardCreatures.Contains(creat))
        {
            OnBoardCreatures.Add(creat);
        }
    }

    public virtual void CreatureRemoved(Creature c)
    {
        if (OnBoardCreatures.Contains(c))
        {
            OnBoardCreatures.Remove(c);
        }
        else if (Reserve.Contains(c))
        {
            Reserve.Remove(c);
        }
        else if (Graveyard.Contains(c))
        {
            Graveyard.Remove(c);
        }
    }

    public virtual void PutInGraveyard(Creature creat)
    {
        Graveyard.Add(creat);
    }

    public void PutInReserve(Creature creat)
    {
        CreatureRemoved(creat);
        Reserve.Add(creat);
        creat.Controller = this;
        creat.PutInReserve();
        if (!MyGame.AllCreatures.Contains(creat))
        {
            MyGame.AllCreatures.Add(creat);
        }
        EventManager.Invoke(GachaEventType.CreatureReserved, this, new CreatureReservedArgs() { BeingReserved = creat, ReserveOwner = this });
    }

    public void RemoveFromReserve(Creature creat)
    {
        Reserve.Remove(creat);
        EventManager.Invoke(GachaEventType.CreatureLeavesReserve, this, new CreatureReservedArgs() { BeingReserved = creat, ReserveOwner = this });
    }
}

public class PlayerArgs
{
    public PlayerType type;
    public IEnumerable<Creature> startingCreatures;
}

public enum PlayerType
{
    HUMAN,
    AI
}
