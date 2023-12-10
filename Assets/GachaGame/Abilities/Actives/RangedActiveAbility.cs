using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RangedActiveAbility : ActiveAbility
{
    public int Range = 1;

    public override void RankUpToOne()
    {
        Range++;
    }

    public override void RankUpToTwo()
    {
        Range++;
    }
}
