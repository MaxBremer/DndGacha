using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class MrSandmanAbility : ActiveAbility
{
    public MrSandmanAbility()
    {
        Name = "MrSandman";
        DisplayName = "Mr. Sandman";
        Description = "Combine all Pale Reincarnations on the battlefield into a Horrific Amalgamation with no abilities, the highest speed of those characters, and the combined attack and health of those characters.";
        MaxCooldown = 3;
    }

    // Can only activate if at least one pale reincarnation
    public override bool IsActivateable()
    {
        return base.IsActivateable() && Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && x.CreatureTypes.Contains("Pale Reincarnation")).Any();
    }

    public override void Trigger(object sender, EventArgs e)
    {
        var relevantCreatures = Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && x.CreatureTypes.Contains("Pale Reincarnation")).ToList();
        var spaceToSummon = relevantCreatures.First().MySpace;
        var atk = 0;
        var health = 0;
        var spd = 0;
        relevantCreatures.ForEach(x => { atk += x.Attack; health += x.Health; spd = Math.Max(spd, x.Speed); });
        var cbase = new CreatureGameBase()
        {
            Initiative = 1,
            Attack = atk,
            Health = health,
            Speed = spd,
            DisplayName = "Horrific Amalgamation",
        };
        cbase.CreatureTypes.Add("Undead");

        var creat = new Creature(cbase);
        creat.InitFromBase();
        creat.SetController(Owner.Controller);

        relevantCreatures.ForEach(x => Owner.MyGame.RemoveCreature(x));
        Owner.MyGame.SummonCreature(creat, spaceToSummon);
    }
}
