using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class UncannyResilienceAbility : AfterMyDeathPassive
{
    public UncannyResilienceAbility()
    {
        Name = "UncannyResillience";
        DisplayName = "UNCANNY RESILLIENCE";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is CreatureDiesArgs dieArgs)
        {
            if (AbilityRank < 1)
            {
                Owner.StatsSet(HealthSet: Owner.Attack);
            }
            else
            {
            Owner.StatsSet(HealthSet: Owner.Attack * 2);
            }
            Owner.MyGame.SummonCreature(Owner, dieArgs.WhereItDied);

            if (AbilityRank == 2)
            {
                var r = new System.Random();
                var keyList = AbilityDatabase.ValidRandomAbilities;
                var key = keyList[r.Next(keyList.Count)];
                var newAbil = (Ability)Activator.CreateInstance(AbilityDatabase.AbilityDictionary[key]);

                Owner.GainAbility(newAbil, true);
            }

            Owner.RemoveAbility(this);
        }
    }

    public override void UpdateDescription()
    {
        Description = "When this character would die, instead set its health equal to " + (AbilityRank < 1 ? "" : "double ") + "its attack and remove this ability." + (AbilityRank < 2 ? "" : " Gain a new random ability in its place.");
    }

    public override void RankUpToTwo()
    {
        
    }

    public override void RankUpToOne()
    {
        
    }
}
