using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class CrushingStepAbility : RangedTargetEnemyAbility
{
    private bool _freeUsage = false;
    private int _dmgAmt = 16;

    public CrushingStepAbility()
    {
        Name = "CrushingStep";
        DisplayName = "Crushing Step";
        MaxCooldown = 0;
        Range = 1;
    }

    public override bool IsActivateable()
    {
        //TODO: Better check if creat has moved this turn? Flag? Applies to Focus Ki too.
        return base.IsActivateable() && (Owner.SpeedLeft >= Owner.Speed || AbilityRank == 2);
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

    public override void UpdateDescription()
    {
        string prefix = AbilityRank < 2 ? "Only usable if this character hasn't moved this turn. " : "";
        Description = prefix + "Select an enemy character in range " + Range + ". Deal " + _dmgAmt + " damage to them. If killed, move into their space, and you may act again.";
    }

    public override void RankUpToOne()
    {
        _dmgAmt += 8;
    }

    public override void RankUpToTwo()
    {
    }
}
