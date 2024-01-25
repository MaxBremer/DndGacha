using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ObsidianSkinAbility : BeforeAttackAbility
{
    private int _dmgBonusAmount = 0;

    public ObsidianSkinAbility()
    {
        Name = "ObsidianSkin";
        DisplayName = "Obsidian Skin";
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
                atkArgs.DamageToDeal += _dmgBonusAmount;
            }
            if(atkArgs.Target == Owner)
            {
                atkArgs.DamageToDeal = 0;
                atkArgs.DamageToTake += _dmgBonusAmount;
            }
        }
    }

    public override void UpdateDescription()
    {
        string midPart = _dmgBonusAmount < 1 ? "" : " and deals +" + _dmgBonusAmount + " damage";
        Description = "Cannot be damaged" + midPart + " in melee combat.";
    }

    public override void RankUpToOne()
    {
        _dmgBonusAmount++;
    }

    public override void RankUpToTwo()
    {
        _dmgBonusAmount += 2;
    }
}