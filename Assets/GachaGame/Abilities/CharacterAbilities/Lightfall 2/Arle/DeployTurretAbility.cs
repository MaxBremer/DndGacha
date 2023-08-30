using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DeployTurretAbility : RangedUnblockedPointTargetAbility
{
    private static CreatureGameBase _turretBase = GetTurretBase();

    public DeployTurretAbility()
    {
        Name = "DeployTurret";
        DisplayName = "Deploy Turret";
        Description = "Create a 2/5/3 turret with a C0 Ranged Attack 3. Increase the cooldown of this ability by 1.";
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
            MaxCooldown += 1;
        }
    }

    private static CreatureGameBase GetTurretBase()
    {
        var ret = new CreatureGameBase()
        {
            Initiative = 1,
            Speed = 2,
            Attack = 3,
            Health = 5,
            Name = "ArleTurret",
            DisplayName = "Turret",
        };

        ret.CreatureTypes.Add("Turret");

        return ret;
    }

    private static Creature GetTurret()
    {
        var tur = new Creature(_turretBase);
        tur.InitFromBase();

        var atkAbil = new RangedAttackEnemiesAbility() { Range = 3, Name = "TurretFireCannon", DisplayName = "Fire Cannon!", Description = "Ranged Attack 3" };
        tur.GainAbility(atkAbil, true);

        return tur;
    }
}
