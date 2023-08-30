using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FlameStrikeAbility : RangedMultiplePointTargetAbility
{
    private const int DAMAGE_AMOUNT = 5;

    public FlameStrikeAbility()
    {
        Name = "FlameStrike";
        DisplayName = "Flame Strike";
        Description = "Choose three points in Range 9. All characters in range 1 of any of these points take " + DAMAGE_AMOUNT + " damage.";
        Range = 9;
        MaxCooldown = 4;
        NumTargets = 3;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        var spaces = new GridSpace[] { GetChoiceNum(1), GetChoiceNum(2), GetChoiceNum(3) };

        foreach (var creat in Owner.MyGame.AllCreatures)
        {
            if(creat.IsOnBoard && InRangeOfAny(spaces, creat))
            {
                creat.TakeDamage(DAMAGE_AMOUNT, Owner);
            }
        }
    }

    private GridSpace GetChoiceNum(int num)
    {
        return (ChoicesNeeded.Where(x => x.Caption == "Target" + num).First() as PointTargetChoice).TargetSpace;
    }

    private bool InRangeOfAny(GridSpace[] spaces, Creature c)
    {
        bool ret = false;

        foreach (var sp in spaces)
        {
            ret |= GachaGrid.IsInRange(c, sp, 1);
        }

        return ret;
    }
}
