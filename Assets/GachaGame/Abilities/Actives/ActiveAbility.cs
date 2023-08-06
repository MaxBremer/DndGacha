using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAbility : Ability
{
    public int MaxCooldown;
    public int Cooldown;

    public override void InitAbility()
    {
        base.InitAbility();
        Cooldown = MaxCooldown;
    }

    // This is like actually what happens when the player clicks the button for the ability.
    public virtual void Activate()
    {
        if(!IsActivateable())
        {
            Debug.LogWarning("WARNING: Attempted to Activate non-activateable ability. This shouldn't happen. Only make activateable abilities available to activate.");
        }
        else
        {
            if (ChoicesNeeded.Count < 1)
            {
                ExternalTrigger(this, new EventArgs());
            }
            else
            {
                ChoiceManager.TriggerBasicPlayerDecision(this);
            }
            Cooldown = MaxCooldown;
        }
    }

    public virtual void CooldownTick()
    {
        if (Cooldown > 0)
        {
            LowerCooldown(1);
        }
    }

    public void LowerCooldown(int amount)
    {
        Cooldown = Math.Max(0, Cooldown - amount);
    }

    public virtual bool IsActivateable()
    {
        return Cooldown == 0 && ChoiceManager.ValidChoicesExist(ChoicesNeeded);
    }

    public override Ability CreateCopy()
    {
        var copy = base.CreateCopy();
        ((ActiveAbility)copy).Cooldown = Cooldown;
        ((ActiveAbility)copy).MaxCooldown = MaxCooldown;
        return copy;
    }
}
