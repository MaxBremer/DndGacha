using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ArtifactLifeBuffAbility : CreatureAffectingAuraWhileOnboardAbility
{
    private int _spd, _atk, _health;

    public ArtifactLifeBuffAbility(int spdBuff = 1, int atkBuff = 1, int healthBuff = 5)
    {
        Name = "ArtifactLifeBuff";
        DisplayName = "Life's Blessing";
        _spd = spdBuff;
        _atk = atkBuff;
        _health = healthBuff;
    }

    public override void UpdateDescription()
    {
        Description = "Your other creatures have +" + _spd + "/+" + _health + "/+" + _atk;
    }

    public override void ApplyEffectToCreature(Creature c)
    {
        c.StatsChange(AtkChg: _atk, HealthChg: _health, SpeedChg: _spd, arePermanentStats: false);
    }

    public override void RemoveEffectFromCreature(Creature c)
    {
        c.StatsChange(AtkChg: -1 * _atk, HealthChg: -1 * _health, SpeedChg: -1 * _spd, arePermanentStats: false);
    }

    public override bool ShouldCreatureBeEffected(Creature c)
    {
        return c != Owner && c.Controller == Owner.Controller;
    }
}
