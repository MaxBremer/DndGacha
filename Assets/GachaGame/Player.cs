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

    public bool CanCallThisTurn = false;

    public PlayerType Type;

    public List<Creature> OnBoardCreatures = new List<Creature>();

    public List<Creature> Graveyard = new List<Creature>();

    public List<Creature> Reserve = new List<Creature>();

    public List<GridSpace> ValidInitSpaces = new List<GridSpace>();

    public Game MyGame;

    public virtual void StartMakingChoices(Ability abil)
    {
        ChoiceManager.MakeChoicesRandomly(abil.ChoicesNeeded);
        ChoiceManager.AbilityChoicesMade(abil);
    }

    public virtual void CreatureCalled(Creature creat)
    {
        if (Reserve.Contains(creat))
        {
            RemoveFromReserve(creat);
            OnBoardCreatures.Add(creat);
            CanCallThisTurn = false;
        }
        else
        {
            Debug.LogWarning("WARNING: Player tried to call creature that was not in reserve.");
        }
    }

    public void PutInReserve(Creature creat)
    {
        Reserve.Add(creat);
        creat.Controller = this;
        EventManager.Invoke("CreatureReserved", this, new CreatureReservedArgs() { BeingReserved = creat, ReserveOwner = this });
    }

    public void RemoveFromReserve(Creature creat)
    {
        Reserve.Remove(creat);
        EventManager.Invoke("CreatureLeavesReserve", this, new CreatureReservedArgs() { BeingReserved = creat, ReserveOwner = this });
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
