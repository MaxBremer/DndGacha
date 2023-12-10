using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class LifespunArtifactAbility : RangedUnblockedPointTargetAbility
{

    private int _spd = 1;
    private int _atk = 1;
    private int _health = 5;

    public LifespunArtifactAbility()
    {
        Name = "LifespunArtifact";
        DisplayName = "Lifespun Artifact";
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

    public override void UpdateDescription()
    {
        Description = "Summon a 0/10/0 artifact in range " + Range + " with \"P: Your other creatures have " + (AbilityRank < 2 ? "+1/+5/+1" : "+2/+8/+2") + "\"";
    }

    public override void RankUpToOne()
    {
        MaxCooldown--;
    }

    public override void RankUpToTwo()
    {
        _spd++;
        _atk++;
        _health += 3;
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
        //cBase.Abilities.Add("ArtifactLifeBuff");

        var creat = new Creature(cBase);
        creat.InitFromBase();
        creat.SetController(Owner.Controller);

        creat.GainAbility(new ArtifactLifeBuffAbility(_spd, _atk, _health));

        return creat;
    }
}
