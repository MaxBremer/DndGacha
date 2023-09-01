using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public bool ReserveTriggerAdded = false, OnboardTriggerAdded = false, GraveyardTriggerAdded = false;

    public string Name;

    public string DisplayName;

    public string Description;

    public Creature Owner;

    public List<Choice> ChoicesNeeded;

    public Game MyGame => Owner.MyGame;

    public virtual void InitAbility() 
    {
        ChoicesNeeded = new List<Choice>();
        if (Owner.IsOnBoard)
        {
            AddOnboardTriggers();
        }
        if (Owner.InReserve)
        {
            AddReserveTriggers();
        }
        if (Owner.InGraveyard)
        {
            AddGraveyardTriggers();
        }
    }

    public bool AllChoicesMade()
    {
        foreach (var choice in ChoicesNeeded)
        {
            if (!choice.ChoiceMade)
            {
                return false;
            }
        }

        return true;
    }

    public void ClearChoices()
    {
        foreach (var choice in ChoicesNeeded)
        {
            choice.ClearChoice();
        }
    }

    public void ClearAllTriggers()
    {
        RemoveOnboardTriggers();
        RemoveReserveTriggers();
        RemoveGraveyardTriggers();
    }

    public virtual void Trigger(object sender, EventArgs e) { }

    public virtual void ExternalTrigger(object sender, EventArgs e)
    {
        if (!AllChoicesMade())
        {
            ChoiceManager.MakeChoicesRandomly(ChoicesNeeded, this);
            // If all choices still aren't made, then at least one has no valid choices and the trigger is cancelled.
            if (!AllChoicesMade())
            {
                return;
            }
        }
        var beforeArgs = new BeforeAbilityTriggerArgs();
        EventManager.Invoke("BeforeAbilityTrigger", this, beforeArgs);
        if (!beforeArgs.Countered)
        {
            for (int i = 0; i < beforeArgs.NumberOfTriggers; i++)
            {
                Trigger(sender, e);
                EventManager.Invoke("AfterAbilityTrigger", this, new EventArgs());
            }
        }
        PostExternalTrigger();
    }

    public virtual void ConditionalTrigger(object sender, EventArgs e)
    {
        ExternalTrigger(sender, e);
    }

    public virtual void PostExternalTrigger() 
    {
        ClearChoices();
    }

    public virtual void AddReserveTriggers() { ReserveTriggerAdded = true; }

    public virtual void RemoveReserveTriggers() { ReserveTriggerAdded = false; }

    public virtual void AddOnboardTriggers() { OnboardTriggerAdded = true; }

    public virtual void RemoveOnboardTriggers() { OnboardTriggerAdded = false; }

    public virtual void AddGraveyardTriggers() { GraveyardTriggerAdded = true; }

    public virtual void RemoveGraveyardTriggers() { GraveyardTriggerAdded = false; }

    public virtual void OnGained() { }

    public virtual void OnLost() { }

    public virtual Ability CreateCopy()
    {
        var copy = (Ability)Activator.CreateInstance(this.GetType());

        copy.Name = Name;
        copy.DisplayName = DisplayName;
        copy.Description = Description;
        copy.Owner = Owner;

        copy.InitAbility();

        return copy;
    }
}
