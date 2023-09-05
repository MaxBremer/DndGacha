﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EnthrallAbility : RangedTargetEnemyAbility
{
    public EnthrallAbility()
    {
        Name = "Enthrall";
        DisplayName = "Enthrall";
        Description = "Select an enemy character within Range 1. It is now in your control, until it is no longer within Range 1 of this character at the start of its turn.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            var enthrall = new EnthralledAbility(Owner.Controller, Owner);
            creatChoice.TargetCreature.GainAbility(enthrall);
        }
    }
}

public class EnthralledAbility : PassiveAbility
{
    private Player _boss;
    private Player _previous;
    private Creature _stayNear;

    public EnthralledAbility(Player newController, Creature stayNear)
    {
        _boss = newController;
        _stayNear = stayNear;
        Name = "Enthralled";
        DisplayName = "Enthralled";
        Description = "Controlled until I start my turn not in Range 1 of my enthraller.";
    }

    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening("StartOfTurn", CheckForEnd);
    }

    private void CheckForEnd(object sender, EventArgs e)
    {
        if(e is TurnStartArgs startArgs && startArgs.PlayerWhoseTurnIsStarting == Owner.Controller.MyPlayerIndex && (!GachaGrid.IsInRange(Owner, _stayNear, 1)))
        {
            Owner.RemoveAbility(this);
        }
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening("StartOfTurn", CheckForEnd);
    }

    public override void OnGained()
    {
        base.OnGained();
        _previous = Owner.Controller;
        Owner.SetController(_boss);
    }

    public override void OnLost()
    {
        base.OnLost();
        Owner.SetController(_previous);
    }
}