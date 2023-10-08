using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ObsidianSkinAbility : BeforeAttackAbility
{
    public ObsidianSkinAbility()
    {
        Name = "ObsidianSkin";
        DisplayName = "Obsidian Skin";
        Description = "Cannot be damaged in melee combat";
        Priority = 3;
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(sender is Creature c && e is AttackArgs atkArgs && !atkArgs.IsRanged && (c == Owner || atkArgs.Target == Owner))
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(sender is Creature c && e is AttackArgs atkArgs)
        {
            if(c == Owner)
            {
                atkArgs.DamageToTake = 0;
            }
            if(atkArgs.Target == Owner)
            {
                atkArgs.DamageToDeal = 0;
            }
        }
    }
}