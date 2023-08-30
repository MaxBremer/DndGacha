using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DashAbility : ActiveAbility
{
    private int _lastSpeed = 0;

    public DashAbility()
    {
        Name = "Dash";
        DisplayName = "Dash";
        Description = "Double this characters speed this turn";
        MaxCooldown = 0;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        _lastSpeed = Owner.Speed;
        Owner.StatsChange(SpeedChg: _lastSpeed);
        EventManager.StartListening("EndOfTurn", LoseSpeed);
    }

    private void LoseSpeed(object sender, EventArgs e)
    {
        Owner.StatsChange(SpeedChg: -1 * _lastSpeed);
        _lastSpeed = 0;
        EventManager.StopListening("EndOfTurn", LoseSpeed);
    }
}
