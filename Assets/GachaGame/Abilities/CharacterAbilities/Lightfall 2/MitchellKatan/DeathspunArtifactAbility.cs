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

    public override void UpdateDescription()
    {
        Description = "Summon a 0/10/0 artifact in range 2 that summons " + (AbilityRank < 2 ? "a hound" : "two hounds") + " at the end of your turns.";
    }

    public override void RankUpToOne()
    {
        MaxCooldown--;
    }

    public override void RankUpToTwo()
    {
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
        //cBase.Abilities.Add("ArtifactSummonHound");

        var creat = new Creature(cBase);
        creat.InitFromBase();
        creat.SetController(Owner.Controller);

        creat.GainAbility(new ArtifactSummonHoundAbility(AbilityRank < 2 ? 1 : 2));

        return creat;
    }
}
