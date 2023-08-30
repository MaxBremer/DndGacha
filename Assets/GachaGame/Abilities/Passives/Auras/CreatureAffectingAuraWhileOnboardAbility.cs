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
        EventManager.StartListening("AfterCreatureDies", RefreshAura);
        EventManager.StartListening("CreatureReserved", RefreshAura);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening("CreatureSummoned", RefreshAura);
        EventManager.StopListening("AfterCreatureDies", RefreshAura);
        EventManager.StopListening("CreatureReserved", RefreshAura);
        ClearAura(null, null);
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

    public abstract bool ShouldCreatureBeEffected(Creature c);

    public abstract void ApplyEffectToCreature(Creature c);

    public abstract void RemoveEffectFromCreature(Creature c);
}
