using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class NobleFriendAbility : BeforeDamageAbility
{
    public NobleFriendAbility()
    {
        Name = "NobleFriend";
        DisplayName = "Noble Friend";
        Description = "If an adjacent friendly character would take lethal damage, this character dies instead, and the adjacent character takes no damage.";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is TakingDamageArgs dmgArgs && sender is Creature c && Owner != c && Owner.Controller == c.Controller && GachaGrid.IsInRange(c, Owner, 1) && dmgArgs.DamageAmount >= c.Health)
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is TakingDamageArgs dmgArgs)
        {
            dmgArgs.DamageAmount = 0;
            Owner.Die();
        }
    }
}
