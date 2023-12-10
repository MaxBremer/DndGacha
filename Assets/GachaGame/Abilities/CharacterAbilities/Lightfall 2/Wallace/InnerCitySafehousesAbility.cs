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
        MaxCooldown = 2;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.GainTag(CreatureTag.HIDDEN);
            bool doImmune = false;
            if(AbilityRank >= 1)
            {
                doImmune = true;
                creatChoice.TargetCreature.GainTag(CreatureTag.IMMUNE);
            }
            creatChoice.TargetCreature.GainHiddenAbility(new RemoveHiddenEndOfTurn(doImmune));
        }
    }

    public override void RankUpToOne()
    {
    }

    public override void UpdateDescription()
    {
        Description = "Choose a character. Give them Hidden (Cannot be targeted by attacks/abilities) " + (AbilityRank < 1 ? "" : "and Immune ") + "until the end of their next turn.";
    }
}

public class RemoveHiddenEndOfTurn : MyTurnEndPassive
{
    private bool _firstTurnEnded = false;
    private bool _immuneAlso;

    public RemoveHiddenEndOfTurn(bool doImmune)
    {
        Name = "RemoveHidden";
        DisplayName = "Safehouse";
        _immuneAlso = doImmune;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (!_firstTurnEnded)
        {
            _firstTurnEnded = true;
            UpdateDescription();
        }
        else
        {
            if (_immuneAlso)
            {
                Owner.LoseTag(CreatureTag.IMMUNE);
            }
            Owner.LoseTag(CreatureTag.HIDDEN);
            Owner.RemoveHiddenAbility(this);
        }
    }

    public override void UpdateDescription()
    {
        Description = "Hidden " + (_immuneAlso ? "and Immune " : "") + "until the end of my next turn " + (_firstTurnEnded ? "(This turn!)" : "(Next turn!)");
    }
}
