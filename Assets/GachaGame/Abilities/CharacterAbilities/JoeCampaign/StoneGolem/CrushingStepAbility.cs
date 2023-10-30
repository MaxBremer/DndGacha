using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class CrushingStepAbility : RangedTargetEnemyAbility
{
    private bool _freeUsage = false;

    public CrushingStepAbility()
    {
        Name = "CrushingStep";
        DisplayName = "Crushing Step";
        Description = "Only usable if this character hasn't moved this turn. Select an enemy character in range 1. Deal 16 damage to them. If killed, move into their space, and you may act again.";
        MaxCooldown = 0;
        Range = 1;
    }

    public override bool IsActivateable()
    {
        //TODO: Better check if creat has moved this turn? Flag? Applies to Focus Ki too.
        return base.IsActivateable() && Owner.SpeedLeft >= Owner.Speed;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            var targetSpace = creatChoice.TargetCreature.MySpace;
            creatChoice.TargetCreature.TakeDamage(16, Owner);
            if (creatChoice.TargetCreature.State == CreatureState.GRAVEYARD)
            {
                Owner.MyGame.GameGrid.TeleportTo(Owner, targetSpace);
                _freeUsage = true;
            }
        }
    }

    public override void PostActivation()
    {
        MidActivation = false;
        Cooldown = MaxCooldown;
        if (_freeUsage)
        {
            _freeUsage = false;
        }
        else
        {
            //Owner.CanAct = false;
            Owner.Acted();
        }
        _activatedThisTurn = true;
    }
}
