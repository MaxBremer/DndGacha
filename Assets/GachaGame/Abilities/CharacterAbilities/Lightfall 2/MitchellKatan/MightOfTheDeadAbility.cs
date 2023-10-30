using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class MightOfTheDeadAbility : FriendlyNonselfCreatureSummonedPassive
{
    private bool _affectingHealth = true;

    public MightOfTheDeadAbility()
    {
        Name = "MightOfTheDead";
        DisplayName = "Might of the Dead";
        Description = "When you summon a creature, double this characters health. Swap this ability to effect attack (swaps on each trigger).";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (_affectingHealth)
        {
            Owner.StatsChange(HealthChg: Owner.Health);
        }
        else
        {
            Owner.StatsChange(AtkChg: Owner.Attack);
        }
        _affectingHealth = !_affectingHealth;
    }
}
