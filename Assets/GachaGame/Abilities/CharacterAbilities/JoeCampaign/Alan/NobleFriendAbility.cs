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
        Priority = 5;
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
            if(AbilityRank == 0)
            {
                Owner.Die();
            }
            else if(AbilityRank == 1)
            {
                Owner.RemoveAbility(this);
            }
            else
            {
                int amount = dmgArgs.DamageAmount / 2;
                Owner.TakeDamage(amount, dmgArgs.DamageDealer);
            }
            dmgArgs.DamageAmount = 0;
        }
    }

    public override void UpdateDescription()
    {
        string MiddleSection = AbilityRank == 0 ? "dies" : (AbilityRank == 1 ? "loses this ability" : "takes half that damage");
        Description = "If an adjacent friendly character would take lethal damage, this character " + MiddleSection + " instead, and the adjacent character takes no damage.";
    }
}
