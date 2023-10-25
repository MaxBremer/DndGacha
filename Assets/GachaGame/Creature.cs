using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Creature
{
    private bool _canAct;

    public Creature(CreatureGameBase baseToInit = null)
    {
        if(baseToInit != null)
        {
            MyCreatureBase = baseToInit;
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

    public HashSet<string> CreatureTypes = new HashSet<string>();

    public CreatureState State;

    /*public bool CanAct
    {
        get => _canAct && (!HasTag(CreatureTag.CANT_ACT));
        set
        {
            _canAct = value;
            if (!_canAct)
            {
                EventManager.Invoke(GachaEventType.CreatureActed, this, new EventArgs());
            }
        }
    }*/

    public bool CanAct { 
        // TODO: Better way of handling CANT_ACT.
        get => _canAct && (!HasTag(CreatureTag.CANT_ACT)); 
        set => _canAct = value; 
    }

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

    public List<PassiveAbility> HiddenAbilities { get; } = new List<PassiveAbility>();

    public IEnumerable<Ability> AllAbilities => Abilities.Concat<Ability>(HiddenAbilities);

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
        SpeedLeft = MyCreatureBase.Speed;
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

        foreach (var type in MyCreatureBase.CreatureTypes)
        {
            CreatureTypes.Add(type);
        }

        HiddenAbilities.Clear();
        // TODO: Hidden abilities in creature base? I don't think so but maybe.

        DisplayName = MyCreatureBase.DisplayName;
    }

    public void StartOfTurnRefresh(bool doStunTagCheck = true)
    {
        var stnTag = Tags.Where(x => x.TagType == CreatureTag.STUNNED);
        if (stnTag.Any() && doStunTagCheck)
        {
            Tags.Remove(stnTag.First());
            SpeedLeft = 0;
            CanAct = false;
        }
        else
        {
            SpeedLeft = Speed;
            CanAct = !HasTag(CreatureTag.CANT_ACT);
        }

        if (HasTag(CreatureTag.SNOOZING))
        {
            LoseTag(CreatureTag.SNOOZING);
        }
    }

    public void EndOfTurn()
    {
        AbilitiesTick();
    }

    public void AbilitiesTick()
    {
        foreach (var abil in Abilities)
        {
            if (abil is ActiveAbility active)
            {
                active.CooldownTick();
            }
        }
    }

    public void Acted()
    {
        CanAct = false;

        EventManager.Invoke(GachaEventType.CreatureActed, this, new EventArgs());
    }

    public void LowerAbilityCooldownsAmount(int num)
    {
        foreach (var abil in Abilities)
        {
            if (abil is ActiveAbility active)
            {
                active.LowerCooldown(num);
            }
        }
    }

    public void Summoned()
    {
        State = CreatureState.ONBOARD;
        foreach (var abil in AllAbilities)
        {
            abil.AddOnboardTriggers();
            if (abil.GraveyardTriggerAdded)
                abil.RemoveGraveyardTriggers();
            if(abil.ReserveTriggerAdded)
                abil.RemoveReserveTriggers();
        }

        GainTag(CreatureTag.SNOOZING);
        CanAct = false;
    }

    public void DeathTriggerChanges()
    {
        //State = CreatureState.GRAVEYARD;
        foreach (var abil in AllAbilities)
        {
            abil.AddGraveyardTriggers();
            if (abil.OnboardTriggerAdded)
                abil.RemoveOnboardTriggers();
            if (abil.ReserveTriggerAdded)
                abil.RemoveReserveTriggers();
        }
    }

    public void RemoveAllTriggers()
    {
        foreach (var abil in AllAbilities)
        {
            abil.RemoveGraveyardTriggers();
            abil.RemoveOnboardTriggers();
            abil.RemoveReserveTriggers();
        }
    }

    public void StatsChange(int AtkChg = 0, int HealthChg = 0, int SpeedChg = 0, int InitChg = 0, bool arePermanentStats = true)
    {
        var changeArgs = new StatChangeArgs() { AttackChange = AtkChg, HealthChange = HealthChg, SpeedChange = SpeedChg, InitChange = InitChg, AreStatsPermanent = arePermanentStats };
        EventManager.Invoke(GachaEventType.BeforeCreatureStatsChange, this, changeArgs);
        Attack = Math.Max(0, Attack + changeArgs.AttackChange);
        
        Speed = Math.Max(0, Speed + changeArgs.SpeedChange);
        SpeedLeft = Math.Max(0, SpeedLeft + changeArgs.SpeedChange);
        Initiative = Math.Max(0, Initiative + changeArgs.InitChange);

        var healthMaxDiff = MaxHealth - Health;
        if (changeArgs.HealthChange > 0)
        {
            MaxHealth += changeArgs.HealthChange;
            Health += changeArgs.HealthChange;
        }else if(changeArgs.HealthChange < 0)
        {
            var amtLost = changeArgs.HealthChange * -1;
            if (amtLost <= healthMaxDiff)
            {
                MaxHealth += changeArgs.HealthChange;
            }
            else
            {
                MaxHealth += changeArgs.HealthChange;
                var amtHealthLost = amtLost - healthMaxDiff;
                Health -= amtHealthLost;
            }
        }
        MaxHealth = Math.Max(0, MaxHealth);
        Health = Math.Max(0, Health);

        EventManager.Invoke(GachaEventType.AfterCreatureStatsChange, this, changeArgs);

        if (Health == 0 && !InGraveyard)
        {
            Die();
        }
    }

    public void StatsSet(int AtkSet = -1, int HealthSet = -1, int SpeedSet = -1, int InitSet = -1)
    {
        Attack = AtkSet >= 0 ? AtkSet : Attack;
        Health = HealthSet >= 0 ? HealthSet : Health;
        MaxHealth = HealthSet >= 0 ? HealthSet : Health;
        Initiative = InitSet >= 0 ? InitSet : Initiative;
        Speed = SpeedSet >= 0 ? SpeedSet : Speed;
        SpeedLeft = SpeedSet >= 0 ? SpeedSet : Speed;
        //TODO: Stats set event maybe?

        if (Health == 0 && !InGraveyard)
        {
            Die();
        }
    }

    public void PutInReserve()
    {
        State = CreatureState.RESERVE;

        if (IsOnBoard)
        {
            MyGame.GameGrid.CreatureLeavesSpace(this);
        }

        LeaveBoard();

        foreach (var abil in AllAbilities)
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
        var candidates = MyGame.AllCreatures.Where(isValid).ToList();
        var args = new BasicAttackTargetingArgs() { ValidAttackTargets = candidates, CreatureAttacking = this, };
        EventManager.Invoke(GachaEventType.CreatureSelectingAttackTargets, this, args);
        return args.ValidAttackTargets;
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
        return WhereTag(tag).Any();
    }

    public IEnumerable<Tag> WhereTag(CreatureTag tag)
    {
        return Tags.Where(x => x.TagType == tag);
    }

    public void GainTag(CreatureTag tag)
    {
        GainTag(new Tag(tag));
    }

    public void GainTag(Tag tag)
    {
        Tags.Add(tag);
    }

    public void LoseTag(CreatureTag tag)
    {
        // Only loses FIRST instance of tag.

        var targetTag = Tags.Where(x => x.TagType == tag).FirstOrDefault();
        if(targetTag != null)
        {
            LoseTag(targetTag);
        }
    }

    public void LoseTag(Tag tag)
    {
        Tags.Remove(tag);
    }

    public void SetController(Player p)
    {
        if(Controller != null && Controller != p)
        {
            Controller.CreatureRemoved(this);
        }
        Controller = p;
        if (IsOnBoard)
        {
            Controller.CreatureSummoned(this);
        }
        if (InReserve)
        {
            Controller.PutInReserve(this);
        }
        if (InGraveyard)
        {
            Controller.PutInGraveyard(this);
        }
    }

    public void BasicAttack(Creature target)
    {
        if (CanAct)
        {
            AttackTarget(target);
            /*CanAct = false;*/
            Acted();
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
        EventManager.Invoke(GachaEventType.BeforeDamage, this, dmgArgs);
        if (!HasTag(CreatureTag.IMMUNE))
        {
            Health -= dmgArgs.DamageAmount;
            if (Health <= 0)
            {
                Health = 0;
                Die();
            }
            EventManager.Invoke(GachaEventType.AfterDamage, this, dmgArgs);
        }
    }

    public void Heal(int healingAmount)
    {
        var healingArgs = new TakingDamageArgs() { DamageAmount = healingAmount };
        EventManager.Invoke(GachaEventType.BeforeHealing, this, healingArgs);
        Health = Math.Min(MaxHealth, Health + healingAmount);
        EventManager.Invoke(GachaEventType.AfterHealing, this, healingArgs);
    }

    public void AttackTarget(Creature target, bool ranged = false)
    {
        var atkArgs = new AttackArgs() { Target = target, IsRanged = ranged, DamageToDeal = Attack, DamageToTake = target.Attack };
        EventManager.Invoke(GachaEventType.BeforeAttack, this, atkArgs);
        if (atkArgs.Target != null)
        {
            atkArgs.Target.TakeDamage(atkArgs.DamageToDeal, this);
            if (!atkArgs.IsRanged && !atkArgs.Target.HasTag(CreatureTag.DEFENSELESS))
            {
                TakeDamage(atkArgs.DamageToTake, atkArgs.Target);
            }
            EventManager.Invoke(GachaEventType.AfterAttack, this, atkArgs);
        }
    }

    public void Die()
    {
        if(State == CreatureState.GRAVEYARD)
        {
            // Already dead, don't do anything.
            return;
        }

        var deathSpot = MySpace;
        EventManager.Invoke(GachaEventType.BeforeCreatureDies, this, new CreatureDiesArgs() { CreatureDied = this, WhereItDied = deathSpot });

        if (State == CreatureState.ONBOARD)
        {
            Controller.MyGame.GameGrid.CreatureLeavesSpace(this);
            Controller.OnBoardCreatures.Remove(this);
        }
        Controller.PutInGraveyard(this);
        State = CreatureState.GRAVEYARD;

        LeaveBoard();

        DeathTriggerChanges();
        EventManager.Invoke(GachaEventType.AfterCreatureDies, this, new CreatureDiesArgs() { CreatureDied = this, WhereItDied = deathSpot });

        if (IsPrime)
        {
            IsPrime = false;
            Controller.MyGame.GainPoint(Controller.MyGame.GetOpponents(Controller));
        }
    }

    public void LeaveBoard()
    {
        // On leaving board, lose all hidden abilities.
        var tempList = new List<PassiveAbility>();
        tempList.AddRange(HiddenAbilities);
        foreach (var hAbil in tempList)
        {
            RemoveHiddenAbility(hAbil);
        }

        EventManager.Invoke(GachaEventType.CreatureLeavesBoard, this, new CreatureDiesArgs() { CreatureDied = this });
    }

    public Creature CreateCopy(bool keepPrime = false)
    {
        var copy = new Creature()
        {
            MyCreatureBase = MyCreatureBase,
            Attack = Attack,
            MaxHealth = MaxHealth,
            Health = Health,
            Speed = Speed,
            SpeedLeft = SpeedLeft,
            Initiative = Initiative,
            DisplayName = DisplayName,
            State = State,
        };

        copy.IsPrime = keepPrime ? IsPrime : false;

        foreach (var tag in Tags)
        {
            copy.Tags.Add(tag.Copy());
        }

        foreach (var type in CreatureTypes)
        {
            copy.CreatureTypes.Add(type);
        }

        foreach (var abil in Abilities)
        {
            copy.GainAbility(abil.CreateCopy());
        }

        foreach (var abil in HiddenAbilities)
        {
            copy.GainHiddenAbility(abil.CreateCopy() as PassiveAbility);
        }

        copy.SetController(Controller);

        return copy;
    }

    public void RemoveAbility(Ability abil)
    {
        Abilities.Remove(abil);
        abil.OnLost();
        abil.Owner = null;
        abil.ClearAllTriggers();
        EventManager.Invoke(GachaEventType.LostAbility, this, new AbilityChangeArgs() { AbilityChanged = abil });
    }

    public void GainAbility(Ability abil, bool initAbil = true)
    {
        Abilities.Add(abil);
        abil.Owner = this;
        if (initAbil)
        {
            abil.InitAbility();
            EventManager.Invoke(GachaEventType.GainedAbility, this, new AbilityChangeArgs() { AbilityChanged = abil });
        }
        abil.OnGained();
    }

    public void GainAbility(string abil, bool initAbil = false)
    {
        var a = Activator.CreateInstance(AbilityDatabase.AbilityDictionary[abil]);
        if(a is Ability ability)
        {
            GainAbility(ability, initAbil);
        }
    }

    public bool HasAbility(string Name)
    {
        return Abilities.Where(x => x.Name == Name).Any();
    }

    public void GainHiddenAbility(PassiveAbility abil)
    {
        HiddenAbilities.Add(abil);
        abil.Owner = this;
        abil.InitAbility();
    }

    public void RemoveHiddenAbility(PassiveAbility abil)
    {
        HiddenAbilities.Remove(abil);
        abil.Owner = null;
        abil.ClearAllTriggers();
    }
}

public enum CreatureState
{
    NONE,
    ONBOARD,
    GRAVEYARD,
    RESERVE,
}
