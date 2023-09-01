using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RangedTargetFriendlyAbility : RangedTargetAbility
{
    public override void InitAbility()
    {
        base.InitAbility();
        var targetChoice = ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault();
        if (targetChoice != null && targetChoice is CreatureTargetChoice creatChoice)
        {
            Func<Creature, bool> isValid = x => x.Controller == Owner.Controller && x != Owner && GachaGrid.IsInRange(Owner, x, Range);
            creatChoice.IsValidCreature = isValid;
        }
    }
}
