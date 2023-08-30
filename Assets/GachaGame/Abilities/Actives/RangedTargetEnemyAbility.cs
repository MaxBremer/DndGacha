using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RangedTargetEnemyAbility : RangedTargetAbility
{
    public override void InitAbility()
    {
        base.InitAbility();
        var targetChoice = ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault();
        if (targetChoice != null && targetChoice is CreatureTargetChoice creatChoice)
        {
            Func<Creature, bool> isValid = x => x.Controller != Owner.Controller && GachaGrid.IsInRange(Owner, x, Range);
            creatChoice.IsValidCreature = isValid;
        }
    }
}
