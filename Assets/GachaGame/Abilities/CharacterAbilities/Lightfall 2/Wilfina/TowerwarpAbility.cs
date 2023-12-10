using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class TowerwarpAbility : WhenImSummonedAbility
{
    private int TowerRange = 1;

    public TowerwarpAbility()
    {
        Name = "Towerwarp";
        DisplayName = "Towerwarp";
    }

    public override void InitAbility()
    {
        base.InitAbility();
        Func<GridSpace, bool> isValid = x => !x.isBlocked;
        ChoicesNeeded.Add(new PointTargetChoice() { Caption = "Target", IsValidSpace = isValid });
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is CreatureSummonArgs sumArgs && sumArgs.BeingSummoned == Owner && ChoiceManager.ValidChoicesExist(ChoicesNeeded, this))
        {
            ChoiceManager.TriggerBasicPlayerDecision(this);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade)
        {
            Owner.MyGame.GameGrid.TeleportTo(Owner, pointChoice.TargetSpace);

            var towerSpaces = new List<GridSpace>();
            towerSpaces.Add(pointChoice.TargetSpace);
            towerSpaces.AddRange(Owner.MyGame.GameGrid.GetSpacesInRange(pointChoice.TargetSpace, TowerRange, true));
            towerSpaces.ForEach(x => x.Tags.Add(SpaceTag.AZURETOWER));
        }
    }

    public override void UpdateDescription()
    {
        Description = "When this character is summoned, choose a square on the board to teleport it to. Squares in range " + TowerRange + " (with diagonals) of that space become the Azure Tower.";
    }

    public override void RankUpToOne()
    {
        TowerRange++;
    }

    public override void RankUpToTwo()
    {
        TowerRange++;
    }
}
