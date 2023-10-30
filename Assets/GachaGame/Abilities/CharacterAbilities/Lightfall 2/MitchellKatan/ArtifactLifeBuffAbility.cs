using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ArtifactLifeBuffAbility : CreatureAffectingAuraWhileOnboardAbility
{
    public ArtifactLifeBuffAbility()
    {
        Name = "ArtifactLifeBuff";
        DisplayName = "Life's Blessing";
        Description = "Your other creatures have +1/+5/+1";
    }

    public override void ApplyEffectToCreature(Creature c)
    {
        c.StatsChange(AtkChg: 1, HealthChg: 5, SpeedChg: 1, arePermanentStats: false);
    }

    public override void RemoveEffectFromCreature(Creature c)
    {
        c.StatsChange(AtkChg: -1, HealthChg: -5, SpeedChg: -1, arePermanentStats: false);
    }

    public override bool ShouldCreatureBeEffected(Creature c)
    {
        return c != Owner && c.Controller == Owner.Controller;
    }
}
