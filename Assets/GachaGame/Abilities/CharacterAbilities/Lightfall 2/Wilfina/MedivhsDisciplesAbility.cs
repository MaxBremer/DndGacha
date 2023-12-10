using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class MedivhsDisciplesAbility : AfterMyDeathPassive
{
    private int HealthAmount = 1;
    private bool SetHealthLow = true;

    public MedivhsDisciplesAbility()
    {
        Name = "MedivhsDisciples";
        DisplayName = "Medivh's Disciples";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is CreatureDiesArgs dieArgs)
        {
            var c = GetAlexander();
            Owner.MyGame.SummonCreature(c, dieArgs.WhereItDied);
            if (SetHealthLow)
            {
                c.StatsSet(HealthSet: HealthAmount);
            }
            c.GainAbility(new DustGraftedBodyAbility(HealthAmount), true);
        }
    }

    public override void RankUpToOne()
    {
        HealthAmount++;
    }

    public override void RankUpToTwo()
    {
        SetHealthLow = false;
    }

    public override void UpdateDescription()
    {
        string midSection = SetHealthLow ? "Set its health to " + HealthAmount + " and g" : "G";
        Description = "When this character dies, summon an Alexander de Venecia. " + midSection + "ive it \"P: At the end of each turn, this gains " + HealthAmount + " health\"";
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
