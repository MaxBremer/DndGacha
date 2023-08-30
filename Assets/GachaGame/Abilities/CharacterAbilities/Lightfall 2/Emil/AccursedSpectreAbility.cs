using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AccursedSpectreAbility : AfterCreatureDiesWhileOnboardAbility
{
    public AccursedSpectreAbility()
    {
        Name = "AccursedSpectre";
        DisplayName = "Accursed Spectre";
        Description = "If a character under my Hexblades curse dies, it is revived it under your control at full health and dies at the end of its next turn.";
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
            Owner.MyGame.SummonCreature(dieArgs.CreatureDied, dieArgs.WhereItDied);
            dieArgs.CreatureDied.SetController(Owner.Controller);
            dieArgs.CreatureDied.Health = dieArgs.CreatureDied.MaxHealth;
            dieArgs.CreatureDied.GainHiddenAbility(new DieAtEndOfNextTurn());
            dieArgs.CreatureDied.CanAct = true;
            dieArgs.CreatureDied.SpeedLeft = dieArgs.CreatureDied.Speed;
        }
    }

    private class DieAtEndOfNextTurn : MyTurnEndPassive
    {
        private bool _firstTurnEnded = false;

        public override void Trigger(object sender, EventArgs e)
        {
            if (!_firstTurnEnded)
            {
                _firstTurnEnded = true;
            }
            else
            {
                Owner.Die();
            }
        }
    }
}
