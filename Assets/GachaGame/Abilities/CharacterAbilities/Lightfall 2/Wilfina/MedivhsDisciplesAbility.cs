using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MedivhsDisciplesAbility : AfterMyDeathPassive
{
    public MedivhsDisciplesAbility()
    {
        Name = "MedivhsDisciples";
        DisplayName = "Medivh's Disciples";
        Description = "When this character dies, summon an Alexander de Venecia. Set its health to 1 and give it \"P: At the end of each turn, this gains 1 health\"";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is CreatureDiesArgs dieArgs)
        {
            var c = GetAlexander();
            Owner.MyGame.SummonCreature(c, dieArgs.WhereItDied);
            c.StatsSet(HealthSet: 1);
            c.GainAbility("DustGraftedBody", true);
        }
    }

    private Creature GetAlexander()
    {
        var cbase = new CreatureGameBase()
        {
            Initiative = 3,
            Speed = 2,
            Health = 10,
            Attack = 1,
            DisplayName = "Alexander de Venecia",
            Abilities = new List<string>() { "PaleReincarnation", "DeathAffinity", "MrSandman"},
        };
        cbase.CreatureTypes.Add("Undead");
        cbase.CreatureTypes.Add("Pale Reincarnation");

        var creat = new Creature(cbase);
        creat.InitFromBase();
        creat.SetController(Owner.Controller);

        return creat;
    }
}
