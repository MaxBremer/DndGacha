using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class InnerCitySafehousesAbility : TargetAnyCreatureAbility
{
    public InnerCitySafehousesAbility()
    {
        Name = "InnerCitySafehouses";
        DisplayName = "Inner-City Safehouses";
        Description = "Choose a character. Give them Hidden (Cannot be targeted by attacks/abilities) until the end of their next turn.";
        MaxCooldown = 2;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.GainTag(CreatureTag.HIDDEN);
            creatChoice.TargetCreature.GainHiddenAbility(new RemoveHiddenEndOfTurn());
        }
    }
}

public class RemoveHiddenEndOfTurn : MyTurnEndPassive
{
    private bool _firstTurnEnded = false;

    public RemoveHiddenEndOfTurn()
    {
        Name = "RemoveHidden";
        DisplayName = "Safehouse";
        Description = "Hidden until the end of my next turn (Next turn!)";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (!_firstTurnEnded)
        {
            _firstTurnEnded = true;
            Description = "Hidden until the end of my next turn (This turn!)";
        }
        else
        {
            Owner.LoseTag(CreatureTag.HIDDEN);
            Owner.RemoveHiddenAbility(this);
        }
    }
}
