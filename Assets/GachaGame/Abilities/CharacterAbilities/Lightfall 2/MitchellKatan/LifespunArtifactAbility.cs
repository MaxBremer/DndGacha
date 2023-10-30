using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class LifespunArtifactAbility : RangedUnblockedPointTargetAbility
{
    public LifespunArtifactAbility()
    {
        Name = "LifespunArtifact";
        DisplayName = "Lifespun Artifact";
        Description = "Summon a 0/10/0 artifact in range 2 with \"P: Your other creatures have +1/+5/+1\"";
        MaxCooldown = 3;
        Range = 2;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            Owner.MyGame.SummonCreature(GetArtifact(), pointChoice.TargetSpace);
        }
    }

    private Creature GetArtifact()
    {
        var cBase = new CreatureGameBase()
        {
            Attack = 0,
            Health = 10,
            Speed = 0,
            Initiative = 2,
            DisplayName = "Lifespun Artifact",
        };
        cBase.CreatureTypes.Add("Artifact");
        cBase.Abilities.Add("ArtifactLifeBuff");

        var creat = new Creature(cBase);
        creat.InitFromBase();
        creat.SetController(Owner.Controller);

        return creat;
    }
}
