using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class DeathspunArtifactAbility : RangedUnblockedPointTargetAbility
{
    public DeathspunArtifactAbility()
    {
        Name = "DeathspunArtifact";
        DisplayName = "Deathspun Artifact";
        Description = "Summon a 0/10/0 artifact in range 2 with \"P: At the end of your turn, summon a 3 / 1 / 4 undead hound adjacent to this\"";
        MaxCooldown = 3;
        Range = 2;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
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
            DisplayName = "Deathspun Artifact",
        };
        cBase.CreatureTypes.Add("Artifact");
        cBase.Abilities.Add("ArtifactSummonHound");

        var creat = new Creature(cBase);
        creat.InitFromBase();
        creat.SetController(Owner.Controller);

        return creat;
    }
}
