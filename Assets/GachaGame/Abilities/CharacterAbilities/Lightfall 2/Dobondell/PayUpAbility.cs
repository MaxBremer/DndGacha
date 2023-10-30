using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class PayUpAbility : RangedAttackEnemiesAbility
{
    public PayUpAbility()
    {
        Name = "PayUp";
        DisplayName = "PAY UP";
        Description = "Choose a character in Range 2. Gain 1 attack, ranged attack it, and increase its current cooldowns by 1.";
        MaxCooldown = 2;
        Range = 2;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            Owner.StatsChange(AtkChg: 1);
            Owner.AttackTarget(creatChoice.TargetCreature, true);
            foreach (var abil in creatChoice.TargetCreature.Abilities)
            {
                if(abil is ActiveAbility active)
                {
                    active.Cooldown += 1;
                }
            }
        }
    }
}
