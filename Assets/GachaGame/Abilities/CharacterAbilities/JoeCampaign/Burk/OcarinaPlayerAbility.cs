using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OcarinaPlayerAbility : BeforeAttackAbility
{
    public OcarinaPlayerAbility()
    {
        Name = "OcarinaPlayer";
        DisplayName = "Ocarina Player";
        Description = "Attacks made by friendly creatures within Range 1 deal 2 additional damage.";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is AttackArgs && sender is Creature c && Owner != c && Owner.Controller == c.Controller && GachaGrid.IsInRange(c, Owner, 1))
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is AttackArgs atkArgs)
        {
            atkArgs.DamageToDeal += 2;
        }
    }
}
