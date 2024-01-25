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
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is TakingDamageArgs dmgArgs)
        {
            var origAmount = dmgArgs.DamageAmount;
            var amount = -1 * dmgArgs.DamageAmount;

            if(AbilityRank >= 1)
            {
                amount = amount / 2;
            }

            Owner.StatsChange(HealthChg: amount);
            dmgArgs.DamageAmount = Math.Max(0, origAmount + amount);

            if(AbilityRank == 2)
            {
                dmgArgs.DamageDealer.TakeDamage(amount, Owner);
            }
        }
    }

    public override void UpdateDescription()
    {
        Description = "Whenever this creature takes damage, it also decreases its Maximum Health " + (AbilityRank < 1 ? "to match." : (AbilityRank < 2 ? "by half that amount rounded down." : "by half that amount rounded down. The creature that did the damage also takes damage equal to that halved amount."));
    }

    public override void RankUpToOne()
    {
    }

    public override void RankUpToTwo()
    {
    }
}
