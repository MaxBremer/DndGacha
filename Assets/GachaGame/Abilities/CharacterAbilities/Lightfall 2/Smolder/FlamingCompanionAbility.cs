using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class FlamingCompanionAbility : WhenImCalledAbility
{
    public FlamingCompanionAbility()
    {
        Name = "FlamingCompanion";
        DisplayName = "Flaming Companion";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        var potentialBuzz = Owner.Controller.Reserve.Where(x => x.DisplayName == "Buzz").FirstOrDefault();
        var potentialSpace = Owner.Controller.ValidInitSpaces.Where(x => !x.isBlocked).FirstOrDefault();
        if(potentialBuzz != null && potentialSpace != null)
        {
            Owner.MyGame.CallCharacter(potentialBuzz, potentialSpace, Owner.Controller);
            if(AbilityRank >= 1)
            {
                potentialBuzz.GainTag(CreatureTag.QUICKSTRIKE);
                if(AbilityRank == 2)
                {
                    potentialBuzz.StatsChange(HealthChg: 3);
                }
            }
        }
    }

    public override void UpdateDescription()
    {
        string endTag = AbilityRank < 1 ? "" : (AbilityRank < 2 ? " Give her Quickstrike." : "Give her Quickstrike and +3 health.");
        Description = "When this character is called, Buzz is called as well if she is in reserve and there is space available." + endTag;
    }

    public override void RankUpToOne()
    {
    }

    public override void RankUpToTwo()
    {
    }
}
