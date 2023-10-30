using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class TowerlockedAbility : MyValidMovesFoundPassive
{
    public TowerlockedAbility()
    {
        Name = "Towerlocked";
        DisplayName = "Towerlocked";
        Description = "This creature cannot move outside the Azure Tower.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is ValidMovesFoundForCreatArgs moveArgs)
        {
            var toDelete = new List<GridSpace>();
            foreach (var sp in moveArgs.ValidMovesWithPaths.Keys)
            {
                if (!sp.Tags.Contains(SpaceTag.AZURETOWER))
                {
                    toDelete.Add(sp);
                }
            }

            toDelete.ForEach(x => moveArgs.ValidMovesWithPaths.Remove(x));
        }
    }
}
