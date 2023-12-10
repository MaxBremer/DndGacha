using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAbility : Ability
{
    protected bool _activatedThisTurn = false;
    
    public int MaxCooldown;
    public int Cooldown;

    public bool MidActivation = false;

    public override void InitAbility()
    {
        base.InitAbility();
        Cooldown = MaxCooldown;
    }

    // This is like actually what happens when the player clicks the button for the ability.
    public virtual void Activate(bool ignoreConditions = false)
    {
        if(!ignoreConditions && !IsActivateable())
        {
            Debug.LogWarning("WARNING: Attempted to Activate non-activateable ability. This shouldn't happen. Only make activateable abilities available to activate.");
        }
        else
        {
            MidActivation = true;

            TrueActivation();
        }
    }

    public virtual void TrueActivation()
    {
        var activationArgs = new AbilityActivateArgs() { AbilityActivating = this, };
        EventManager.Invoke(GachaEventType.BeforeActiveAbilityActivates, this, activationArgs);
        if (activationArgs.CounterActivation)
        {
            return;
        }

        for (int i = 0; i < activationArgs.NumActivations; i++)
        {
            if (ChoicesNeeded.Count < 1)
            {
                ExternalTrigger(this, new EventArgs());
            }
            else
            {
                ChoiceManager.TriggerBasicPlayerDecision(this);
            }
        }

        EventManager.Invoke(GachaEventType.AfterActiveAbilityActivates, this, activationArgs);
    }

    public virtual void CooldownTick()
    {
        if (Cooldown > 0)
        {
            if (_activatedThisTurn)
            {
                _activatedThisTurn = false;
            }
            else
            {
                LowerCooldown(1);
            }
        }
    }

    public void LowerCooldown(int amount)
    {
        var args = new CooldownEventArgs() { CooldownAmount = amount, AbilityCooled = this, AbilityOwner = Owner };
        EventManager.Invoke(GachaEventType.AbilityCooldownLower, this, args);
        Cooldown = Math.Max(0, Cooldown - args.CooldownAmount);
    }

    public void SetCurrentCooldownTo(int amount)
    {
        Cooldown = amount;
    }

    public virtual void PostActivation()
    {
        MidActivation = false;
        Cooldown = MaxCooldown;
        //Owner.CanAct = false;
        Owner.Acted();
        _activatedThisTurn = true;
    }

    public void CancelActivation()
    {
        MidActivation = false;
        ChoiceManager.CancelActiveChoiceAbility(this);
    }

    public virtual bool IsActivateable()
    {
        return Owner.CanAct && Cooldown == 0 && ChoiceManager.ValidChoicesExist(ChoicesNeeded, this);
    }

    public override void PostExternalTrigger()
    {
        base.PostExternalTrigger();
        if (MidActivation)
        {
            PostActivation();
        }
    }

    public override Ability CreateCopy()
    {
        var copy = base.CreateCopy();
        ((ActiveAbility)copy).Cooldown = Cooldown;
        ((ActiveAbility)copy).MaxCooldown = MaxCooldown;
        return copy;
    }

    public override void RankUpToOne()
    {
        MaxCooldown = Math.Max(MaxCooldown - 1, 0);
    }

    public override void RankUpToTwo()
    {
        MaxCooldown = Math.Max(MaxCooldown - 1, 0);
    }
}
