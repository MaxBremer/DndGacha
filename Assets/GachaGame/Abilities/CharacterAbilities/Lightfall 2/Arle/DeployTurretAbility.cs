using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class DeployTurretAbility : RangedUnblockedPointTargetAbility
{
    public DeployTurretAbility()
    {
        Name = "DeployTurret";
        DisplayName = "Deploy Turret";
        MaxCooldown = 1;
        Range = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            var tur = GetTurret();
            tur.SetController(Owner.Controller);
            Owner.MyGame.SummonCreature(tur, pointChoice.TargetSpace);
            if(AbilityRank < 1)
            {
                MaxCooldown += 1;
            }
        }
    }

    public override void RankUpToOne()
    {
    }

    public override void RankUpToTwo()
    {
        
    }

    public override void UpdateDescription()
    {
        Description = "Create a " + (AbilityRank < 2 ? "2/5/3" : "3/10/4") + " turret with a C0 Ranged Attack 3." + (AbilityRank < 1 ? " Increase the cooldown of this ability by 1." : "");
    }

    private CreatureGameBase GetTurretBase()
    {
        var ret = new CreatureGameBase()
        {
            Initiative = 1,
            Speed = AbilityRank < 2 ? 2 : 3,
            Attack = AbilityRank < 2 ? 3 : 4,
            Health = AbilityRank < 2 ? 5 : 10,
            Name = "ArleTurret",
            DisplayName = "Turret",
        };

        ret.CreatureTypes.Add("Turret");

        return ret;
    }

    private Creature GetTurret()
    {
        var tur = new Creature(GetTurretBase());
        tur.InitFromBase();

        var atkAbil = new RangedAttackEnemiesAbility() { Range = 3, Name = "TurretFireCannon", DisplayName = "Fire Cannon!", Description = "Ranged Attack 3" };
        tur.GainAbility(atkAbil, true);

        return tur;
    }
}
