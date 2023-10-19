using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MightAsWellBeDeadAbility : TargetAnyCreatureAbility
{
    public MightAsWellBeDeadAbility()
    {
        Name = "MightAsWellBeDead";
        DisplayName = "Might as well be dead";
        Description = "Choose a character. Remove them from the game.";
        MaxCooldown = 2;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            Owner.MyGame.RemoveCreature(creatChoice.TargetCreature);
        }
    }
}
