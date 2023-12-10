using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class BestOfABadThingAbility : FriendlyCreatureDiesPassive
{
    public BestOfABadThingAbility()
    {
        Name = "BestOfABadThing";
        DisplayName = "Best of a Bad Thing";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        foreach (var creat in Owner.MyGame.AllCreatures.Where(x => x.Controller == Owner.Controller))
        {
            creat.AbilitiesTick();
            if (AbilityRank >= 1)
            {
                creat.Heal(1);
            }
        }
    }

    public override void RankUpToOne()
    {
        
    }

    public override void UpdateDescription()
    {
        string suffix = AbilityRank < 1 ? "." : " and heal them by 1.";
        suffix += AbilityRank > 1 ? " This triggers twice." : "";
        Description = "When this or another friendly creature dies, reduce the cooldown counters of your creatures by 1" + suffix;
    }
}
