using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RangedTargetMultipleEnemiesAbility : RangedTargetAbility
{
    public int NumChoices = 0;

    public override void InitAbility()
    {
        base.InitAbility();
        ChoicesNeeded.Clear();
        Func<Creature, bool> isValid = x => x.Controller != Owner.Controller && GachaGrid.IsInRange(Owner, x, Range);
        for (int i = 0; i < NumChoices; i++)
        {
            var num = i + 1;
            var newChoice = new CreatureTargetChoice() { Caption = "Target" + num, IsValidCreature = isValid };
            ChoicesNeeded.Add(newChoice);
        }
    }
}
