using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class AgricultureAbility : RangedMultiplePointTargetAbility
{
    public AgricultureAbility()
    {
        Name = "Agriculture";
        DisplayName = "Agriculture";
        Description = "Summon two 0/2/0 plants with Defenseless in range 2.";
        MaxCooldown = 0;
        Range = 2;
        NumTargets = 2;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        for (int i = 0; i < NumTargets; i++)
        {
            var num = i + 1;
            var space = (ChoicesNeeded.Where(x => x.Caption == "Target" + num).First() as PointTargetChoice).TargetSpace;

            Owner.MyGame.SummonCreature(GetPlant(), space);
        }

    }

    private Creature GetPlant()
    {
        var cbase = new CreatureGameBase()
        {
            Initiative = 1,
            Speed = 0,
            Health = 2,
            Attack = 0,
            DisplayName = "Potted Plant",
        };
        cbase.CreatureTypes.Add("Plant");
        cbase.Tags.Add(CreatureTag.DEFENSELESS);

        var creat = new Creature(cbase);
        creat.InitFromBase();
        creat.SetController(Owner.Controller);

        return creat;
    }
}
