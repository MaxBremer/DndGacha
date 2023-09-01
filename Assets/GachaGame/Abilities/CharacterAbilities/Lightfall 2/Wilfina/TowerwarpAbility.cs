using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TowerwarpAbility : WhenImSummonedAbility
{
    private List<GridSpace> _myTower = new List<GridSpace>();

    public TowerwarpAbility()
    {
        Name = "Towerwarp";
        DisplayName = "Towerwarp";
        Description = "When this character is summoned, choose a square on the board to teleport it to. Squares in range 1 (with diagonals) of that space become the Azure Tower.";
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
            towerSpaces.AddRange(Owner.MyGame.GameGrid.GetAdjacents(pointChoice.TargetSpace, true));
            towerSpaces.ForEach(x => x.Tags.Add(SpaceTag.AZURETOWER));
        }
    }
}
