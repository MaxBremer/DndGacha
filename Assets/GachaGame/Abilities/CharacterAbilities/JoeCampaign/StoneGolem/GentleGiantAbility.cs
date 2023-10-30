using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class GentleGiantAbility : PassiveAbility
{
    public GentleGiantAbility()
    {
        Name = "GentleGiant";
        DisplayName = "Gentle Giant";
        Description = "Cannot attack unless below max health";
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

    private void CreatureSelectingTargets(object sender, EventArgs e)
    {
        if(e is BasicAttackTargetingArgs atkArgs && atkArgs.CreatureAttacking == Owner && Owner.Health >= Owner.MaxHealth)
        {
            atkArgs.ValidAttackTargets.Clear();
        }
    }
}
