using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class TowerlockedAbility : CreatureMovesFoundWhileOnboardPassive
{
    private bool iCantMove = true;
    private bool enemiesMoveSlow = false;

    public TowerlockedAbility()
    {
        Name = "Towerlocked";
        DisplayName = "Towerlocked";
        Priority = 1;
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
        else if (e is GridSpaceSpeedWeightArgs gridArgs)
        {
            gridArgs.SpaceSpeedWeight = 2;
        }
    }

    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.BeforeGridSpaceMoveWeightGet, ConditionalTrigger, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening(GachaEventType.BeforeGridSpaceMoveWeightGet, ConditionalTrigger, Priority);
    }

    public override void RankUpToOne()
    {
        enemiesMoveSlow = true;
    }

    public override void RankUpToTwo()
    {
        iCantMove = false;
    }

    public override void UpdateDescription()
    {
        var suffix = (enemiesMoveSlow ? "Enemy creatures move at half speed through the Azure Tower." : "") + (iCantMove ? "" : "Free at last.");
        Description = (iCantMove ? "This creature cannot move outside the Azure Tower. " : "") + suffix;
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(isSpaceSpeedTrigger(sender, e))
        {
            //Debug.Log("is space speed trigger");
            ExternalTrigger(sender, e);
        }
        else if (isMovesFoundTrigger(sender, e))
        {
            //Debug.Log("is moves found trigger");
            ExternalTrigger(sender, e);
        }
    }

    private bool isSpaceSpeedTrigger(object sender, EventArgs e)
    {
        return e is GridSpaceSpeedWeightArgs spdArgs
            && (spdArgs.CreatureMovingThrough != null)
            && (spdArgs.CreatureMovingThrough.Controller != Owner.Controller)
            && enemiesMoveSlow 
            && sender is GridSpace g
            && g.Tags.Contains(SpaceTag.AZURETOWER);
    }

    private bool isMovesFoundTrigger(object sender, EventArgs e)
    {
        return e is ValidMovesFoundForCreatArgs moveArgs
            && moveArgs.CreatureMoving == Owner
            && iCantMove;
    }
}
