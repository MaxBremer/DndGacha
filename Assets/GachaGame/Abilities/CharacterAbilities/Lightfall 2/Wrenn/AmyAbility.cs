using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class AmyAbility : RangedUnblockedPointTargetAbility
{
    private Creature MyAmy = null;

    public AmyAbility()
    {
        Name = "Amy";
        DisplayName = "Amy";
        Description = "Summon a 4/8/1 dog named Amy. There can only ever be one Amy. When she dies, replace this ability with \"Tragic Backstory\".";
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

    private Creature GetAmy()
    {
        var creat = new Creature()
        {
            DisplayName = "Amy",
            Attack = 1,
            MaxHealth = 8,
            Health = 8,
            Speed = 4,
            SpeedLeft = 4,
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
            MyWrenn.RemoveAbility(WrennAmyAbil);
            MyWrenn.GainAbility(new TragicBackstoryAbility(), true);
        }
    }
}
