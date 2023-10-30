using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class FragileAbility : BeforeITakeDamagePassiveAbility
{
    public FragileAbility()
    {
        Name = "Fragile";
        DisplayName = "Fragile";
        Description = "Whenever this creature takes damage, it also decreases its Maximum Health to match.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is TakingDamageArgs dmgArgs)
        {
            var amount = -1 * dmgArgs.DamageAmount;
            Owner.StatsChange(HealthChg: amount);
            dmgArgs.DamageAmount = 0;
        }
    }
}
