using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class AmyAbility : RangedUnblockedPointTargetAbility
{
    private Creature MyAmy = null;
    private int amySpeed = 3;
    private int amyHealth = 8;
    private int amyAtk = 1;

    public AmyAbility()
    {
        Name = "Amy";
        DisplayName = "Amy";
        Range = 1;
        MaxCooldown = 1;
    }

    public override bool IsActivateable()
    {
        return base.IsActivateable() && MyAmy == null;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            var amy = GetAmy();
            amy.SetController(Owner.Controller);
            Owner.MyGame.SummonCreature(amy, pointChoice.TargetSpace);
            MyAmy = amy;
        }
    }

    public override void UpdateDescription()
    {
        Description = "Summon a " + amySpeed + "/" + amyHealth + "/" + amyAtk + " dog named Amy. There can only ever be one Amy. When she dies, replace this ability with \"Tragic Backstory\".";
    }

    public override void RankUpToOne()
    {
        amySpeed++;
        amyAtk++;
    }

    public override void RankUpToTwo()
    {
        amySpeed++;
        amyAtk += 2;
    }

    private Creature GetAmy()
    {
        var creat = new Creature()
        {
            DisplayName = "Amy",
            Attack = amyAtk,
            MaxHealth = amyHealth,
            Health = amyHealth,
            Speed = amySpeed,
            SpeedLeft = amySpeed,
            Initiative = 1,
        };

        creat.CreatureTypes.Add("Animal");

        var abil = new AmyDiesAbility();
        abil.MyWrenn = Owner;
        abil.WrennAmyAbil = this;
        creat.GainAbility(abil, true);

        return creat;
    }

    internal class AmyDiesAbility : AfterMyDeathPassive
    {
        public Creature MyWrenn;
        public Ability WrennAmyAbil;

        public AmyDiesAbility()
        {
            Name = "BetterNot";
            DisplayName = "She Better Not";
            Description = "After this dies, replace its summoner's \"Amy\" ability with \"Tragic Backstory\".";
        }

        public override void Trigger(object sender, EventArgs e)
        {
            if(MyWrenn != null && WrennAmyAbil != null && MyWrenn.HasAbility(WrennAmyAbil))
            MyWrenn.RemoveAbility(WrennAmyAbil);
            MyWrenn.GainAbility(new TragicBackstoryAbility(), true);
        }
    }
}
