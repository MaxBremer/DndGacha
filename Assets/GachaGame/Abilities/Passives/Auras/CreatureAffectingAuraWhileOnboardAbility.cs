using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class CreatureAffectingAuraWhileOnboardAbility : PassiveAbility
{
    public List<Creature> CurrentlyEffectedCreatures = new List<Creature>();

    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening("CreatureSummoned", RefreshAura);
        EventManager.StartListening("AfterCreatureDies", OnCreatureDeath);
        EventManager.StartListening("CreatureReserved", OnCreatureReserved);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening("CreatureSummoned", RefreshAura);
        EventManager.StopListening("AfterCreatureDies", OnCreatureDeath);
        EventManager.StopListening("CreatureReserved", OnCreatureReserved);
    }

    public virtual void RefreshAura(object sender, EventArgs e)
    {
        ClearAura(sender, e);

        foreach (var c in Owner.MyGame.AllCreatures)
        {
            if (ShouldCreatureBeEffected(c))
            {
                ApplyEffectToCreature(c);
                CurrentlyEffectedCreatures.Add(c);
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

    public virtual void OnCreatureDeath(object sender, EventArgs e)
    {
        if(e is CreatureDiesArgs dieArgs && dieArgs.CreatureDied == Owner && Owner.InGraveyard)
        {
            ClearAura(sender, e);
        }
        else
        {
            RefreshAura(sender, e);
        }
    }

    public virtual void OnCreatureReserved(object sender, EventArgs e)
    {
        if (e is CreatureReservedArgs reserveArgs && reserveArgs.BeingReserved == Owner && Owner.InReserve)
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
