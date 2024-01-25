using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class GentleGiantAbility : PassiveAbility
{
    private int _healthGainAmount = 0;

    public GentleGiantAbility()
    {
        Name = "GentleGiant";
        DisplayName = "Gentle Giant";
    }

    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.CreatureSelectingAttackTargets, CreatureSelectingTargets, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening(GachaEventType.CreatureSelectingAttackTargets, CreatureSelectingTargets, Priority);
    }

    public override void UpdateDescription()
    {
        string suffix = (_healthGainAmount > 0 ? " Gain " + _healthGainAmount + " health." : "");
        Description = "Cannot attack unless below max health." + suffix;
    }

    public override void OnGained()
    {
        base.OnGained();
        if(_healthGainAmount > 0)
        {
            Owner.StatsChange(HealthChg: _healthGainAmount);
        }
    }

    public override void RankUpToOne()
    {
        _healthGainAmount += 5;
    }

    public override void RankUpToTwo()
    {
        _healthGainAmount += 5;
    }

    private void CreatureSelectingTargets(object sender, EventArgs e)
    {
        if(e is BasicAttackTargetingArgs atkArgs && atkArgs.CreatureAttacking == Owner && Owner.Health >= Owner.MaxHealth)
        {
            atkArgs.ValidAttackTargets.Clear();
        }
    }
}
