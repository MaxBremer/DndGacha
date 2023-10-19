using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AssassinateAbility : RangedTargetAbility
{
    public AssassinateAbility()
    {
        Name = "Assassinate";
        DisplayName = "Assassinate";
        Description = "Choose a creature in range 4 with 7 or less health. Kill it.";
        MaxCooldown = 5;
        Range = 4;
    }

    public override void InitAbility()
    {
        base.InitAbility();
        var targetChoice = ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault();
        if (targetChoice != null && targetChoice is CreatureTargetChoice creatChoice)
        {
            Func<Creature, bool> isValid = x => x != Owner && GachaGrid.IsInRange(Owner, x, Range) && x.Health <= 7;
            creatChoice.IsValidCreature = isValid;
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.Die();
        }
    }
}
