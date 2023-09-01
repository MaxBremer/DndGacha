using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DeathAffinityAbility : FriendlyNonselfCreatureDiesPassive
{
    public DeathAffinityAbility()
    {
        Name = "DeathAffinity";
        DisplayName = "Death Affinity";
        Description = "When another friendly character dies, reduce all cooldown counters on this character to 0.";
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
    }
}
