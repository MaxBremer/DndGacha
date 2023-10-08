using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DivineSmiteAbility : ActiveAbility
{
    private const int INCREASE_AMOUNT = 3;

    public DivineSmiteAbility()
    {
        Name = "DivineSmite";
        DisplayName = "Divine Smite";
        Description = "The next time this character deals damage, it deals " + INCREASE_AMOUNT + " extra.";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        EventManager.StartListening(GachaEventType.BeforeDamage, IncreaseDamage, Priority);
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
