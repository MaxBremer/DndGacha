using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class AgricultureAbility : RangedMultiplePointTargetAbility
{
    private int plantAtkAmount = 0;
    private int plantHealthAmount = 2;

    public AgricultureAbility()
    {
        Name = "Agriculture";
        DisplayName = "Agriculture";
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

    public override void RankUpToOne()
    {
        plantAtkAmount++;
        plantHealthAmount++;
    }

    public override void RankUpToTwo()
    {
    }

    public override void UpdateDescription()
    {
        Description = "Summon two 0/" + plantHealthAmount + "/" + plantAtkAmount + " plants " + (AbilityRank < 2 ? "with Defenseless " : "") + "in range 2.";
    }

    private Creature GetPlant()
    {
        var cbase = new CreatureGameBase()
        {
            Initiative = 1,
            Speed = 0,
            Health = plantHealthAmount,
            Attack = plantAtkAmount,
            DisplayName = "Potted Plant",
        };
        cbase.CreatureTypes.Add("Plant");
        
        if(AbilityRank < 2)
        {
            cbase.Tags.Add(CreatureTag.DEFENSELESS);
        }

        var creat = new Creature(cbase);
        creat.InitFromBase();
        creat.SetController(Owner.Controller);

        return creat;
    }
}
