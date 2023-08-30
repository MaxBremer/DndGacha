using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TargetSingleFriendlyReserveCreatureAbility : ActiveAbility
{
    public override void InitAbility()
    {
        base.InitAbility();
        Func<Creature, bool> isValid = x => x.Controller == Owner.Controller && x.InReserve;
        ChoicesNeeded.Add(new CreatureTargetChoice() { Caption = "Target", IsValidCreature = isValid });
    }
}
