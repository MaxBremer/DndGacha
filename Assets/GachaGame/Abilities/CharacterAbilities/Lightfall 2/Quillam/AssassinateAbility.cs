using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class AssassinateAbility : RangedTargetAbility
{
    private int _healthMax = 7;

    public AssassinateAbility()
    {
        Name = "Assassinate";
        DisplayName = "Assassinate";
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

    public override void UpdateDescription()
    {
        Description = "Choose a creature in range " + Range + " with " + _healthMax + " or less health. Kill it.";
    }

    public override void RankUpToOne()
    {
        MaxCooldown--;
        Range++;
    }

    public override void RankUpToTwo()
    {
        _healthMax += 2;
    }
}
