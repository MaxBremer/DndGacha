using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class DashAbility : ActiveAbility
{
    private int _lastSpeed = 0;
    private int speedMult = 1;

    public DashAbility()
    {
        Name = "Dash";
        DisplayName = "Dash";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        _lastSpeed = Owner.Speed * speedMult;
        Owner.StatsChange(SpeedChg: _lastSpeed);
        EventManager.StartListening(GachaEventType.EndOfTurn, LoseSpeed, Priority);
    }

    private void LoseSpeed(object sender, EventArgs e)
    {
        Owner.StatsChange(SpeedChg: -1 * _lastSpeed);
        _lastSpeed = 0;
        EventManager.StopListening(GachaEventType.EndOfTurn, LoseSpeed, Priority);
    }

    public override void RankUpToTwo()
    {
        speedMult = 2;
    }

    public override void UpdateDescription()
    {
        Description = (speedMult == 2 ? "Triple" : "Double") + " this characters speed this turn.";
    }
}
