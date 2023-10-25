using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class CreatureAffectingAuraWhileOnboardAbility : AuraAbility
{
    public List<Creature> CurrentlyEffectedCreatures = new List<Creature>();

    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.CreatureSummoned, RefreshAura, Priority);
        EventManager.StartListening(GachaEventType.CreatureLeavesBoard, OnCreatureLeavesBoard, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening(GachaEventType.CreatureSummoned, RefreshAura, Priority);
        EventManager.StopListening(GachaEventType.CreatureLeavesBoard, OnCreatureLeavesBoard, Priority);
    }

    public virtual void RefreshAura(object sender, EventArgs e)
    {
        foreach (var creat in CurrentlyEffectedCreatures)
        {
            if (!ShouldCreatureBeEffected(creat))
            {
                RemoveEffectFromCreature(creat);
                CurrentlyEffectedCreatures.Remove(creat);
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

    public override void OnLost()
    {
        base.OnLost();

        ClearAura(null, null);
    }

    public override void OnGained()
    {
        base.OnGained();

        RefreshAura(null, null);
    }

    public abstract bool ShouldCreatureBeEffected(Creature c);

    public abstract void ApplyEffectToCreature(Creature c);

    public abstract void RemoveEffectFromCreature(Creature c);
}
