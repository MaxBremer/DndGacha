using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class AccursedSpectreAbility : AfterCreatureDiesWhileOnboardAbility
{
    private int NumTurnsUntilDeath = 2;

    public AccursedSpectreAbility()
    {
        Name = "AccursedSpectre";
        DisplayName = "Accursed Spectre";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(e is CreatureDiesArgs dieArgs && dieArgs.CreatureDied.WhereTag(CreatureTag.HEXBLADES_CURSE).Where(x => x.CreatureData == Owner).Any())
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is CreatureDiesArgs dieArgs)
        {
            dieArgs.CreatureDied.SetController(Owner.Controller);
            Owner.MyGame.SummonCreature(dieArgs.CreatureDied, dieArgs.WhereItDied);
            dieArgs.CreatureDied.Health = dieArgs.CreatureDied.MaxHealth;
            dieArgs.CreatureDied.GainAbility(new FadingSpectreAbility(NumTurnsUntilDeath));
            dieArgs.CreatureDied.LoseTag(dieArgs.CreatureDied.WhereTag(CreatureTag.HEXBLADES_CURSE).Where(x => x.CreatureData == Owner).First());
            /*dieArgs.CreatureDied.LoseTag(CreatureTag.SNOOZING);
            dieArgs.CreatureDied.CanAct = true;
            dieArgs.CreatureDied.SpeedLeft = dieArgs.CreatureDied.Speed;*/
            dieArgs.CreatureDied.StartOfTurnRefresh(false);
        }
    }

    public override void RankUpToOne()
    {
        NumTurnsUntilDeath++;
    }

    public override void RankUpToTwo()
    {
        NumTurnsUntilDeath++;
    }

    public override void UpdateDescription()
    {
        Description = "If a character under my Hexblades curse dies, it is revived it under your control at full health and dies in " + NumTurnsUntilDeath + " turns.";
    }

    private class FadingSpectreAbility : MyTurnEndPassive
    {
        private bool _firstTurnEnded = false;
        private int _turnsUntilDeath;

        public FadingSpectreAbility(int deathTurns)
        {
            _turnsUntilDeath = deathTurns;
            Name = "FadingSpectre";
            DisplayName = "Fading Spectre";
            Description = "This dies and loses this ability " + _turnsUntilDeath + " after it was summoned.";
        }

        public override void Trigger(object sender, EventArgs e)
        {
            if (!_firstTurnEnded)
            {
                _firstTurnEnded = true;
            }
            else
            {
                Owner.Die();
                //Owner.RemoveAbility(this);
            }
        }
    }
}
