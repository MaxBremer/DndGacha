using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class RealityIsMineAbility : TargetAnyOtherCreatureAbility
{
    public RealityIsMineAbility()
    {
        Name = "RealityIsMine";
        DisplayName = "Reality is Mine!";
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

            if(AbilityRank >= 1 && creatChoice.TargetCreature.Controller == Owner.Controller)
            {
                creatChoice.TargetCreature.Heal(Owner.Attack);
            }else if(AbilityRank == 2 && creatChoice.TargetCreature.Controller != Owner.Controller)
            {
                Owner.AttackTarget(creatChoice.TargetCreature, true);
            }
        }
    }

    public override void UpdateDescription()
    {
        var suffix = AbilityRank < 1 ? "" : " If it's friendly, heal it an amount equal to this minions attack.";
        suffix += AbilityRank < 2 ? "" : " If it's an enemy, perform a ranged attack on it.";
        Description = "Swap locations with another creature." + suffix;
    }

    public override void RankUpToOne()
    {
    }

    public override void RankUpToTwo()
    {
    }
}
