﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class CreatureAffectingAuraWhileOnboardAbility : AuraAbility
{
    public List<Creature> CurrentlyEffectedCreatures = new List<Creature>();

    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening("CreatureSummoned", RefreshAura);
        EventManager.StartListening("CreatureLeavesBoard", OnCreatureLeavesBoard);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening("CreatureSummoned", RefreshAura);
        EventManager.StopListening("CreatureLeavesBoard", OnCreatureLeavesBoard);
    }

    public virtual void RefreshAura(object sender, EventArgs e)
    {
        foreach (var creat in CurrentlyEffectedCreatures)
        {
            if (!ShouldCreatureBeEffected(creat))
            {
                RemoveEffectFromCreature(creat);
            }
        }

        foreach (var creat in Owner.MyGame.AllCreatures)
        {
            if(!CurrentlyEffectedCreatures.Contains(creat) && ShouldCreatureBeEffected(creat))
            {
                ApplyEffectToCreature(creat);
                CurrentlyEffectedCreatures.Add(creat);
            }
        }
    }

    public virtual void ClearAura(object sender, EventArgs e)
    {
        foreach (var c in CurrentlyEffectedCreatures)
        {
            RemoveEffectFromCreature(c);
        }
        CurrentlyEffectedCreatures.Clear();
    }

    public virtual void OnCreatureLeavesBoard(object sender, EventArgs e)
    {
        if(e is CreatureDiesArgs leftArgs && leftArgs.CreatureDied == Owner && Owner.InGraveyard)
        {
            ClearAura(sender, e);
        }
        else
        {
            RefreshAura(sender, e);
        }
    }

    public abstract bool ShouldCreatureBeEffected(Creature c);

    public abstract void ApplyEffectToCreature(Creature c);

    public abstract void RemoveEffectFromCreature(Creature c);
}
