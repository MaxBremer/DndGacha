using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class LuringPupilsAbility : CreatureMovesFoundWhileOnboardPassive
{
    public LuringPupilsAbility()
    {
        Name = "LuringPupils";
        DisplayName = "Luring Pupils";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(e is ValidMovesFoundForCreatArgs moveArgs && moveArgs.ValidMovesWithPaths.Keys.Where(x => GachaGrid.IsInRange(Owner, x, 3)).Any())
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is ValidMovesFoundForCreatArgs moveArgs)
        {
            var toDelete = new List<GridSpace>();
            foreach (var sp in moveArgs.ValidMovesWithPaths.Keys)
            {
                if (!GachaGrid.IsInRange(Owner, sp, 3))
                {
                    toDelete.Add(sp);
                }
            }

            toDelete.ForEach(x => moveArgs.ValidMovesWithPaths.Remove(x));
        }
    }

    public override void UpdateDescription()
    {
        //Rank 1: If an ENEMY creature can move...
        //Rank 2: ...it must move as close as possible.
        Description = "When moving, if a creature can move within Range 3 of this creature, it must.";
    }
}
