using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TargetSingleEnemyAbility : ActiveAbility
{
    public override void InitAbility()
    {
        base.InitAbility();
        Func<Creature, bool> isValid = c => c.IsOnBoard && c.Controller != Owner.Controller;
        ChoicesNeeded.Add(new CreatureTargetChoice() { IsValidCreature = isValid, Caption = "Target" });
    }
}
