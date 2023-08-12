using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Creature
{
    public CreatureGameBase MyCreatureBase;

    public string DisplayName;

    public int Initiative;

    public int Attack;

    public int Health;

    public int MaxHealth;

    public int Speed;

    public int SpeedLeft;

    public List<CreatureTag> Tags = new List<CreatureTag>();

    public CreatureState State;

    public bool CanAct;

    public bool IsCharacter;

    public bool IsPrime = false;

    public (int, int) Position
    {
        get
        {
            if (MySpace != null)
            {
                return (MySpace.XPos, MySpace.YPos);
            }
            else
            {
                return (-1, -1);
            }
        }
    }

    public GridSpace MySpace = null;

    public bool InReserve => State == CreatureState.RESERVE;

    public bool InGraveyard => State == CreatureState.GRAVEYARD;

    public bool CanBasicAttack => CanAct && GetValidBasicAttackTargets().Count > 0;

    public Player Controller;

    public List<Ability> Abilities { get; } = new List<Ability>();

    public Game MyGame => Controller.MyGame;

    public bool IsOnBoard => MySpace != null;

    public void InitFromBase(CreatureGameBase creatBase = null)
    {
        if(creatBase != null)
        {
            MyCreatureBase = creatBase;
        }

        if(MyCreatureBase == null)
        {
            Debug.LogWarning("WARNING: Cancelled out of creature init from base due to null base.");
            return;
        }

        Attack = MyCreatureBase.Attack;
        Speed = MyCreatureBase.Speed;
        Health = MyCreatureBase.Health;
        MaxHealth = MyCreatureBase.Health;
        Initiative = MyCreatureBase.Initiative;

        Abilities.Clear();
        foreach (var abil in MyCreatureBase.Abilities)
        {
            var instance = Activator.CreateInstance(AbilityDatabase.AbilityDictionary[abil]);
            if (instance is Ability abilInst)
            {
                GainAbility(abilInst, true);
            }
        }

        Tags.Clear();
        Tags.AddRange(MyCreatureBase.Tags);

        DisplayName = MyCreatureBase.DisplayName;
    }

    public void StartOfTurnRefresh()
    {
        SpeedLeft = Speed;
        CanAct = true;
    }

    public void EndOfTurn()
    {
        foreach (var abil in Abilities)
        {
            if (abil is ActiveAbility active)
            {
                active.CooldownTick();
            }
        }
    }

    public void Summoned()
    {
        State = CreatureState.ONBOARD;
        foreach (var abil in Abilities)
        {
            abil.AddOnboardTriggers();
            if (abil.GraveyardTriggerAdded)
                abil.RemoveGraveyardTriggers();
            if(abil.ReserveTriggerAdded)
                abil.RemoveReserveTriggers();
        }
    }

    public void StatsChange(int AtkChg = 0, int HealthChg = 0, int SpeedChg = 0, int InitChg = 0)
    {
        var changeArgs = new StatChangeArgs() { AttackChange = AtkChg, HealthChange = HealthChg, SpeedChange = SpeedChg, InitChange = InitChg };
        EventManager.Invoke("BeforeCreatureStatsChange", this, changeArgs);
        Attack += changeArgs.AttackChange;
        MaxHealth += changeArgs.HealthChange;
        Health += changeArgs.HealthChange;
        Speed += changeArgs.SpeedChange;
        SpeedLeft += changeArgs.SpeedChange;
        Initiative += changeArgs.InitChange;
        EventManager.Invoke("AfterCreatureStatsChange", this, changeArgs);
    }

    public void PutInReserve()
    {
        State = CreatureState.RESERVE;
        foreach (var abil in Abilities)
        {
            abil.AddReserveTriggers();
            if (abil.GraveyardTriggerAdded)
                abil.RemoveGraveyardTriggers();
            if (abil.OnboardTriggerAdded)
                abil.RemoveOnboardTriggers();
        }
    }

    public List<Creature> GetValidBasicAttackTargets()
    {
        Func<Creature, bool> isValid = x => x != this && GachaGrid.IsInRange(this, x, 1) && x.Controller != Controller;
        return MyGame.AllCreatures.Where(isValid).ToList();
    }

    public void BasicAttack(Creature target)
    {
        if (CanAct)
        {
            AttackTarget(target);
            CanAct = false;
        }
        else
        {
            // WARNING: 
            Debug.LogWarning("TRIED TO BASIC ATTACK BUT CANNOT ACT. SHOULD ONLY CALL THIS WHEN CAN ACT.");
        }
    }

    public void TakeDamage(int damageAmount)
    {
        var dmgArgs = new TakingDamageArgs() { DamageAmount = damageAmount };
        EventManager.Invoke("BeforeDamage", this, dmgArgs);
        Health -= dmgArgs.DamageAmount;
        EventManager.Invoke("AfterDamage", this, dmgArgs);
        if(Health <= 0)
        {
            Die();
        }
    }

    public void AttackTarget(Creature target, bool ranged = false)
    {
        var atkArgs = new AttackArgs() { Target = target, IsRanged = ranged };
        EventManager.Invoke("BeforeAttack", this, atkArgs);
        if (atkArgs.Target != null)
        {
            atkArgs.Target.TakeDamage(Attack);
            if (!atkArgs.IsRanged)
            {
                TakeDamage(atkArgs.Target.Attack);
            }
            EventManager.Invoke("AfterAttack", this, atkArgs);
        }
    }

    public void Die()
    {
        if(State == CreatureState.ONBOARD)
        {
            Controller.MyGame.GameGrid.CreatureLeavesSpace(this);
            Controller.OnBoardCreatures.Remove(this);
        }
        Controller.Graveyard.Add(this);
        State = CreatureState.GRAVEYARD;
        foreach (var abil in Abilities)
        {
            abil.RemoveOnboardTriggers();
            abil.AddGraveyardTriggers();
        }
        EventManager.Invoke("CreatureDies", this, new EventArgs());
    }

    public Creature CreateCopy()
    {
        var copy = new Creature()
        {
            MyCreatureBase = MyCreatureBase,
            Attack = Attack,
            MaxHealth = MaxHealth,
            Health = Health,
            Speed = Speed,
            Initiative = Initiative,
            DisplayName = DisplayName,
            State = State,
        };

        copy.Tags.AddRange(Tags);

        return copy;
    }

    public void RemoveAbility(Ability abil)
    {
        Abilities.Remove(abil);
        abil.Owner = null;
        abil.ClearAllTriggers();
        EventManager.Invoke("LostAbility", this, new AbilityChangeArgs() { AbilityChanged = abil });
    }

    public void GainAbility(Ability abil, bool initAbil = false)
    {
        Abilities.Add(abil);
        abil.Owner = this;
        if (initAbil)
        {
            abil.InitAbility();
        }
        EventManager.Invoke("GainedAbility", this, new AbilityChangeArgs() { AbilityChanged = abil });
    }
}

public enum CreatureState
{
    NONE,
    ONBOARD,
    GRAVEYARD,
    RESERVE,
}
