using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class DeathAffinityAbility : FriendlyNonselfCreatureDiesPassive
{
    private int StatChangeAmount = 0;
    private bool DoStatBoost => StatChangeAmount > 0;

    public DeathAffinityAbility()
    {
        Name = "DeathAffinity";
        DisplayName = "Death Affinity";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        foreach (var abil in Owner.Abilities)
        {
            if(abil is ActiveAbility active)
            {
                active.SetCurrentCooldownTo(0);
            }
        }

        if (DoStatBoost)
        {
            Owner.StatsChange(AtkChg: StatChangeAmount, HealthChg: StatChangeAmount);
        }
    }

    public override void RankUpToOne()
    {
        StatChangeAmount++;
    }

    public override void RankUpToTwo()
    {
        StatChangeAmount++;
    }

    public override void UpdateDescription()
    {
        Description = "When another friendly character dies, reduce all cooldown counters on this character to 0" + (DoStatBoost ? " and give it " + StatChangeAmount + " attack and health." : ".");
    }
}
