using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class MethodicalAbility : PassiveAbility
{
    private int _turnsLeft = 4;

    public MethodicalAbility()
    {
        Name = "Methodical";
        DisplayName = "Methodical";
    }

    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.CreatureActed, Acted, Priority);
        EventManager.StartListening(GachaEventType.CreatureMoved, Moved, Priority);
        EventManager.StartListening(GachaEventType.EndOfTurn, TurnEnd, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveGraveyardTriggers();
        EventManager.StopListening(GachaEventType.CreatureActed, Acted, Priority);
        EventManager.StopListening(GachaEventType.CreatureMoved, Moved, Priority);
        EventManager.StopListening(GachaEventType.EndOfTurn, TurnEnd, Priority);
    }

    public override void UpdateDescription()
    {
        var thingCantDo = AbilityRank < 1 ? "act" : "use active abilities";
        var suffix = AbilityRank < 2 ? "" : " After " + _turnsLeft + " turns, lose this ability and damn the innocent.";
        Description = "Cannot " + thingCantDo + " and move on the same turn." + suffix;
    }

    private void Acted(object sender, EventArgs e)
    {
        if(sender is Creature c && c == Owner)
        {
            Owner.GainTag(CreatureTag.CANT_MOVE);
            Owner.GainHiddenAbility(new RemoveCantMoveEndOfTurn());
        }
    }

    private void Moved(object sender, EventArgs e)
    {
        if(sender is Creature c && c == Owner)
        {
            bool cantAct = AbilityRank < 1;
            Owner.GainTag(cantAct ? CreatureTag.CANT_ACT : CreatureTag.CANT_ACTIVATE);
            if (cantAct)
            {
                Owner.GainHiddenAbility(new RemoveCantActEndOfTurn());
            }
            else
            {
                Owner.GainHiddenAbility(new RemoveCantActivateEndOfTurn());
            }
        }
    }

    private void TurnEnd(object sender, EventArgs e)
    {
        if (AbilityRank < 2) return;

        if (e is TurnEndArgs turnArgs && turnArgs.PlayerWhoseTurnIsEnding == Owner.Controller.MyPlayerIndex)
        {
            _turnsLeft--;
            if(_turnsLeft <= 0)
            {
                Owner.RemoveAbility(this);
            }
        }
    }
}

public class RemoveCantActEndOfTurn : MyTurnEndPassive
{
    public RemoveCantActEndOfTurn()
    {
        Name = "RemoveCantAct";
        Description = "Shouldn't see this.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.LoseTag(CreatureTag.CANT_ACT);
        Owner.RemoveHiddenAbility(this);
    }
}

public class RemoveCantActivateEndOfTurn : MyTurnEndPassive
{
    public RemoveCantActivateEndOfTurn()
    {
        Name = "RemoveCantAct";
        Description = "Shouldn't see this.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.LoseTag(CreatureTag.CANT_ACTIVATE);
        Owner.RemoveHiddenAbility(this);
    }
}
