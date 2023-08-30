using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SecondWindAbility : ActiveAbility
{
    private bool _alreadyUsedThisBattle = false;

    public SecondWindAbility()
    {
        Name = "SecondWind";
        DisplayName = "Second Wind";
        Description = "Restore this character to full health. This can only be used once per battle.";
        MaxCooldown = 0;
    }

    public override bool IsActivateable()
    {
        return base.IsActivateable() && !_alreadyUsedThisBattle;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.Heal(Owner.MaxHealth);
        _alreadyUsedThisBattle = true;
    }
}
