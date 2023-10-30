using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class MightOfTheFamilyAbility : TargetSingleEnemyAbility
{
    public MightOfTheFamilyAbility()
    {
        Name = "MightOfTheFamily";
        DisplayName = "Might of the Family";
        Description = "Choose an enemy creature. Deal 1 damage to them, then summon a 2/6/2 Goon that attacks them immediately if there's room.";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.TakeDamage(1, Owner);
            var validSpaces = Owner.MyGame.GameGrid.GetAdjacents(creatChoice.TargetCreature.MySpace, false);
            if (validSpaces.Count > 0)
            {
                var r = new Random();
                var targetSpace = validSpaces[r.Next(validSpaces.Count)];
                var goon = GetGoon();
                Owner.MyGame.SummonCreature(goon, targetSpace);
                goon.AttackTarget(creatChoice.TargetCreature);
            }
        }
    }

    private Creature GetGoon()
    {
        CreatureGameBase goonBase = new CreatureGameBase()
        {
            Initiative = 1,
            Speed = 2,
            Health = 6,
            Attack = 2,
        };

        var creat = new Creature(goonBase);
        creat.InitFromBase();
        creat.SetController(Owner.Controller);

        return creat;
    }
}
