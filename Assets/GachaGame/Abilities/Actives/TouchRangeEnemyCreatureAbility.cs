using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TouchRangeEnemyCreatureAbility : ActiveAbility
{
    public override void InitAbility()
    {
        base.InitAbility();
        Func<Creature, bool> isValid = x => x != Owner && GachaGrid.IsInRange(Owner, x, 1) && x.Controller != Owner.Controller;
        ChoicesNeeded.Add(new CreatureTargetChoice() { IsValidCreature = isValid, Caption = "Target" });
    }
}
