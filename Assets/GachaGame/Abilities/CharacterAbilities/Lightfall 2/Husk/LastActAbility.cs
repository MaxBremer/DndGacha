using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class LastActAbility : ActiveAbility
{
    private int NUM_TIMES_ATTACK = 2;
    private int SELF_DAMAGE = 99;

    public LastActAbility()
    {
        Name = "LastAct";
        DisplayName = "Last Act";
        MaxCooldown = 6;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        foreach (var target in MyGame.AllCreatures.Where(x => x.Controller != Owner.Controller))
        {
            for (int i = 0; i < NUM_TIMES_ATTACK; i++)
            {
                Owner.AttackTarget(target, true);
            }
        }
        Owner.TakeDamage(SELF_DAMAGE, Owner);
    }

    public override void UpdateDescription()
    {
        Description = "This character performs " + (AbilityRank < 1 ? "two" : "three") + " ranged attacks on each enemy creature, regardless of range. Then deal " + SELF_DAMAGE + " to this character.";
    }

    public override void RankUpToOne()
    {
        NUM_TIMES_ATTACK++;
    }
}
