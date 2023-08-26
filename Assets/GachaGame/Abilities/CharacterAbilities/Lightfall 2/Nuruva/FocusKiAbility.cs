using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FocusKiAbility : ActiveAbility
{
    public FocusKiAbility()
    {
        Name = "FocusKi";
        DisplayName = "Focus Ki";
        Description = "Only usable if this character has full Speed remaining. This character gains 2 health and 1 attack. This character loses all speed this turn.";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.StatsChange(AtkChg: 1, HealthChg: 2);
        Owner.SpeedLeft = 0;
    }

    public override bool IsActivateable()
    {
        return base.IsActivateable() && Owner.SpeedLeft == Owner.Speed;
    }
}
