using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class DivineSmiteAbility : ActiveAbility
{
    private int INCREASE_AMOUNT = 3;

    public DivineSmiteAbility()
    {
        Name = "DivineSmite";
        DisplayName = "Divine Smite";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        EventManager.StartListening(GachaEventType.BeforeDamage, IncreaseDamage, Priority);
    }

    public override void UpdateDescription()
    {
        Description = "The next time this character deals damage, it deals " + INCREASE_AMOUNT + " extra.";
    }

    public override void RankUpToTwo()
    {
        INCREASE_AMOUNT += 2;
    }

    private void IncreaseDamage(object sender, EventArgs e)
    {
        if(e is TakingDamageArgs dmgArgs && dmgArgs.DamageDealer == Owner && dmgArgs.DamageAmount > 0)
        {
            dmgArgs.DamageAmount += INCREASE_AMOUNT;
            EventManager.StopListening(GachaEventType.BeforeDamage, IncreaseDamage, Priority);
        }
    }
}
