using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class FocusKiAbility : ActiveAbility
{
    private int _healthGain = 2;
    private int _atkGain = 1;

    public FocusKiAbility()
    {
        Name = "FocusKi";
        DisplayName = "Focus Ki";
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

    public override void UpdateDescription()
    {
        Description = "Only usable if this character has full Speed remaining. This character gains " + _healthGain + " health and " + _atkGain + " attack. This character loses all speed this turn.";
    }

    public override void RankUpToOne()
    {
        _healthGain++;
    }

    public override void RankUpToTwo()
    {
        _healthGain++;
        _atkGain++;
    }
}
