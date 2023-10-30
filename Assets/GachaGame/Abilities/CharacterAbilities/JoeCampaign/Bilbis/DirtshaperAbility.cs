using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class DirtshaperAbility : RangedMultiplePointTargetAbility
{
    public DirtshaperAbility()
    {
        Name = "Dirtshaper";
        DisplayName = "Dirtshaper";
        Description = "Summon 3 1-health dirt mounds within range 3. They can't move or act.";
        MaxCooldown = 0;
        Range = 3;
        NumTargets = 3;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        foreach (var item in ChoicesNeeded)
        {
            if(item is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
            {
                Owner.MyGame.SummonCreature(GetDirt(), pointChoice.TargetSpace);
            }
        }
    }

    private Creature GetDirt()
    {
        var cbase = new CreatureGameBase()
        {
            Initiative = 1,
            Health = 1,
            Attack = 0,
            Speed = 0,
            DisplayName = "Dirt Mound",
        };
        cbase.Tags.Add(CreatureTag.CANT_ACT);
        cbase.Tags.Add(CreatureTag.CANT_MOVE);

        var creat = new Creature(cbase);
        creat.InitFromBase();
        creat.SetController(Owner.Controller);

        return creat;
    }
}
