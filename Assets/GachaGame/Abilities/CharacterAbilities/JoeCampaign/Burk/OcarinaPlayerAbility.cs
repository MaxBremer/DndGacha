using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class OcarinaPlayerAbility : BeforeAttackAbility
{
    private int _dmgBoost = 2;
    private int _sortofAuraRange = 1;
    public OcarinaPlayerAbility()
    {
        Name = "OcarinaPlayer";
        DisplayName = "Ocarina Player";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is AttackArgs && sender is Creature c && Owner != c && Owner.Controller == c.Controller && GachaGrid.IsInRange(c, Owner, _sortofAuraRange))
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is AttackArgs atkArgs)
        {
            atkArgs.DamageToDeal += _dmgBoost;
        }
    }

    public override void UpdateDescription()
    {
        Description = "Attacks made by friendly creatures within Range " + _sortofAuraRange + " deal " + _dmgBoost + " additional damage.";
    }

    public override void RankUpToOne()
    {
        _dmgBoost++;
    }

    public override void RankUpToTwo()
    {
        _dmgBoost++;
        _sortofAuraRange++;
    }
}
