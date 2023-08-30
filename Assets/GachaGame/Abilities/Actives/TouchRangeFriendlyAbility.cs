using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TouchRangeFriendlyAbility : ActiveAbility
{
    public int Range = 1;

    public override void InitAbility()
    {
        base.InitAbility();
        Func<Creature, bool> isValid = x => x != Owner && x.Controller == Owner.Controller && GachaGrid.IsInRange(Owner, x, Range);
        ChoicesNeeded.Add(new CreatureTargetChoice() { IsValidCreature = isValid, Caption = "Target" });
    }
}
