using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TouchRangeFriendlyOrSelfAbility : ActiveAbility
{
    public int Range = 1;

    public override void InitAbility()
    {
        base.InitAbility();
        Func<Creature, bool> isValid = x => x.Controller == Owner.Controller && GachaGrid.IsInRange(Owner, x, Range);
        ChoicesNeeded.Add(new CreatureTargetChoice() { IsValidCreature = isValid, Caption = "Target" });
    }
}
