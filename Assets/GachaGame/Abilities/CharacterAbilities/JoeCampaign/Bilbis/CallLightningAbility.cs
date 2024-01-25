using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class CallLightningAbility : RangedPointTargetAbility
{
    private int _splashRange = 2;

    public CallLightningAbility()
    {
        Name = "CallLightning";
        DisplayName = "Call Lightning";
        Range = 6;
        MaxCooldown = 2;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            var relevantCreatures = Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && GachaGrid.IsInRange(x, pointChoice.TargetSpace, _splashRange)).ToList();
            relevantCreatures.ForEach(x => x.TakeDamage(Owner.Attack, Owner));
        }
    }

    public override void UpdateDescription()
    {
        Description = "Choose a tile within range " + Range + ". All creatures within range " + _splashRange + " of that tile take damage equal to my attack.";
    }

    public override void RankUpToOne()
    {
        _splashRange++;
    }

    public override void RankUpToTwo()
    {
        _splashRange++;
        Range++;
    }
}
