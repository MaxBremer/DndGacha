using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Creature
{
    public Creature(CreatureGameBase baseToInit = null)
    {
        if(baseToInit != null)
        {

        }
    }

    public CreatureGameBase MyCreatureBase;

    public string DisplayName;

    public int Initiative;

    public int Attack;

    public int Health;

    public int MaxHealth;

    public int Speed;

    public int SpeedLeft;

    public List<Tag> Tags = new List<Tag>();

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

    public List<Ability> HiddenAbilities { get; } = new List<Ability>();

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
        foreach (var tag in MyCreatureBase.Tags)
        {
            Tags.Add(new Tag(tag));
        }

        DisplayName = MyCreatureBase.DisplayName;
    }

    public void StartOfTurnRefresh()
    {
        var stnTag = Tags.Where(x => x.TagType == CreatureTag.STUNNED);
        if (stnTag.Any())
        {
            Tags.Remove(stnTag.First());
            SpeedLeft = 0;
        }
        else
        {
            SpeedLeft = Speed;
            CanAct = true;
        }
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

    public bool CanActivateAbility(Ability abil)
    {
        return Abilities.Contains(abil) && abil is ActiveAbility activeAbil && activeAbil.IsActivateable();
    }

    public bool CanActivateAbility(int AbilIndex)
    {
        return Abilities.Count > AbilIndex && CanActivateAbility(Abilities[AbilIndex]);
    }

    public bool HasTag(CreatureTag tag)
    {
        return Tags.Where(x => x.TagType == tag).Any();
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

    public void TakeDamage(int damageAmount, Creature damageDealer)
    {
        var dmgArgs = new TakingDamageArgs() { DamageAmount = damageAmount, DamageDealer = damageDealer };
        EventManager.Invoke("BeforeDamage", this, dmgArgs);
        Health -= dmgArgs.DamageAmount;
        EventManager.Invoke("AfterDamage", this, dmgArgs);
        if(Health <= 0)
        {
            Die();
        }
    }

    public void Heal(int healingAmount)
    {
        var healingArgs = new TakingDamageArgs() { DamageAmount = healingAmount };
        EventManager.Invoke("BeforeHealing", this, healingArgs);
        Health = Math.Min(MaxHealth, Health + healingAmount);
        EventManager.Invoke("AfterHealing", this, healingArgs);
    }

    public void AttackTarget(Creature target, bool ranged = false)
    {
        var atkArgs = new AttackArgs() { Target = target, IsRanged = ranged };
        EventManager.Invoke("BeforeAttack", this, atkArgs);
        if (atkArgs.Target != null)
        {
            atkArgs.Target.TakeDamage(Attack, this);
            if (!atkArgs.IsRanged && !atkArgs.Target.HasTag(CreatureTag.DEFENSELESS))
            {
                TakeDamage(atkArgs.Target.Attack, atkArgs.Target);
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

        foreach (var tag in Tags)
        {
            copy.Tags.Add(new Tag(tag.TagType, tag.Data));
        }

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
            EventManager.Invoke("GainedAbility", this, new AbilityChangeArgs() { AbilityChanged = abil });
        }
    }
}

public enum CreatureState
{
    NONE,
    ONBOARD,
    GRAVEYARD,
    RESERVE,
}
