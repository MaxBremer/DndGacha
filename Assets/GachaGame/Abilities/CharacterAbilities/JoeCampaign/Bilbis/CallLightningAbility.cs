using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CallLightningAbility : RangedPointTargetAbility
{
    public CallLightningAbility()
    {
        Name = "CallLightning";
        DisplayName = "Call Lightning";
        Description = "Choose a tile within range 6. All creatures within range 2 of that tile take damage equal to my attack.";
        Range = 6;
        MaxCooldown = 2;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            var relevantCreatures = Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && GachaGrid.IsInRange(x, pointChoice.TargetSpace, 2)).ToList();
            relevantCreatures.ForEach(x => x.TakeDamage(Owner.Attack, Owner));
        }
    }
}
