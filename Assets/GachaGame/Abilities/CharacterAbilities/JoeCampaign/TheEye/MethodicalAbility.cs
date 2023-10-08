using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MethodicalAbility : PassiveAbility
{
    public MethodicalAbility()
    {
        Name = "Methodical";
        DisplayName = "Methodical";
        Description = "Cannot act and move on the same turn.";
    }

    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.CreatureActed, Acted, Priority);
        EventManager.StartListening(GachaEventType.CreatureMoved, Moved, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveGraveyardTriggers();
        EventManager.StopListening(GachaEventType.CreatureActed, Acted, Priority);
        EventManager.StopListening(GachaEventType.CreatureMoved, Moved, Priority);
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
            Owner.GainTag(CreatureTag.CANT_ACT);
            Owner.GainHiddenAbility(new RemoveCantActEndOfTurn());
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
