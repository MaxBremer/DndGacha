using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RealityIsMineAbility : TargetAnyOtherCreatureAbility
{
    public RealityIsMineAbility()
    {
        Name = "RealityIsMine";
        DisplayName = "Reality is Mine!";
        Description = "Swap locations with another creature";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            var targetSpace = creatChoice.TargetCreature.MySpace;
            var myOldSpace = Owner.MySpace;
            Owner.MyGame.GameGrid.CreatureLeavesSpace(creatChoice.TargetCreature);
            Owner.MyGame.GameGrid.TeleportTo(Owner, targetSpace);
            Owner.MyGame.GameGrid.TeleportTo(creatChoice.TargetCreature, myOldSpace);
        }
    }
}
