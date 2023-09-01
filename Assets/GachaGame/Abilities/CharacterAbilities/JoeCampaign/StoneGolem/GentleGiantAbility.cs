using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GentleGiantAbility : PassiveAbility
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
        EventManager.StartListening("CreatureSelectingAttackTargets", CreatureSelectingTargets);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening("CreatureSelectingAttackTargets", CreatureSelectingTargets);
    }

    private void CreatureSelectingTargets(object sender, EventArgs e)
    {
        if(e is BasicAttackTargetingArgs atkArgs && atkArgs.CreatureAttacking == Owner && Owner.Health >= Owner.MaxHealth)
        {
            atkArgs.ValidAttackTargets.Clear();
        }
    }
}
