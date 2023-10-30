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
        Description = "When this character is called, Buzz is called as well if she is in reserve and there is space available.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        var potentialBuzz = Owner.Controller.Reserve.Where(x => x.DisplayName == "Buzz").FirstOrDefault();
        var potentialSpace = Owner.Controller.ValidInitSpaces.Where(x => !x.isBlocked).FirstOrDefault();
        if(potentialBuzz != null && potentialSpace != null)
        {
            Owner.MyGame.CallCharacter(potentialBuzz, potentialSpace, Owner.Controller);
        }
    }
}
