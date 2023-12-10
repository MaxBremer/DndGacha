using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class SecondWindAbility : ActiveAbility
{
    private int numTimesAllowed = 1;
    private int numTimesUsedThusFar = 0;

    public SecondWindAbility()
    {
        Name = "SecondWind";
        DisplayName = "Second Wind";
        MaxCooldown = 0;
    }

    public override bool IsActivateable()
    {
        return base.IsActivateable() && numTimesUsedThusFar < numTimesAllowed;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.Heal(Owner.MaxHealth);
        numTimesUsedThusFar++;
    }

    public override void RankUpToOne()
    {
        numTimesAllowed++;
    }

    public override void RankUpToTwo()
    {
        numTimesAllowed++;
    }

    public override void UpdateDescription()
    {
        var numTimesSt = numTimesAllowed == 1 ? "once" : (numTimesAllowed == 2 ? "twice" : "thrice");
        Description = "Restore this character to full health. This can only be used " + numTimesSt + " per battle.";
    }
}
