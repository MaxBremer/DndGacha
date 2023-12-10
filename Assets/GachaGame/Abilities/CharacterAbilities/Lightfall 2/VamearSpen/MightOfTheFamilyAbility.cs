using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class MightOfTheFamilyAbility : TargetSingleEnemyAbility
{
    private int goonAtk = 2;
    private int goonHealth = 6;
    private int goonSpd = 2;
    private int dmgAmount = 1;

    public MightOfTheFamilyAbility()
    {
        Name = "MightOfTheFamily";
        DisplayName = "Might of the Family";
        MaxCooldown = 1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            //creatChoice.TargetCreature.TakeDamage(1, Owner);
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

    public override void UpdateDescription()
    {
        Description = "Choose an enemy creature. Summon a " + goonSpd + "/" + goonHealth + "/" + goonAtk + " Goon adjacent to them that attacks them immediately if there's room.";
    }

    public override void RankUpToOne()
    {
        goonHealth += 2;
        goonAtk++;
    }

    public override void RankUpToTwo()
    {
        goonHealth += 3;
        goonAtk += 2;
        goonSpd++;
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
