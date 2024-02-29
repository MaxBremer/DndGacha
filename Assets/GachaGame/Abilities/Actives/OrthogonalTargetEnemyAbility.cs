using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OrthogonalTargetEnemyAbility : ActiveAbility
{
    public override void InitAbility()
    {
        base.InitAbility();

        var chooseWhichToActivate = new OptionSelectChoice() { Caption = "Options" };
        chooseWhichToActivate.Options.Add(new ChoiceOption() { OptionName = "North", ConditionOfPresentation = GetValidForDirection("North") });
        chooseWhichToActivate.Options.Add(new ChoiceOption() { OptionName = "South", ConditionOfPresentation = GetValidForDirection("South") });
        chooseWhichToActivate.Options.Add(new ChoiceOption() { OptionName = "East", ConditionOfPresentation = GetValidForDirection("East") });
        chooseWhichToActivate.Options.Add(new ChoiceOption() { OptionName = "West", ConditionOfPresentation = GetValidForDirection("West") });

        ChoicesNeeded.Add(chooseWhichToActivate);
    }

    internal virtual Creature GetTargetForDir(ChoiceOption opt)
    {
        var dir = opt.OptionName;

        var condOptChoice = ChoicesNeeded.Where(x => x.Caption == "Options").First() as OptionSelectChoice;

        var possibleTargets = Owner.MyGame.AllCreatures.Where(x =>
        {
            if (!x.IsOnBoard)
            {
                return false;
            }
            bool possibility = false;
            switch (condOptChoice.ChosenOption.OptionName)
            {
                case "North":
                    possibility = x.MySpace.XPos == Owner.MySpace.XPos && x.MySpace.YPos > Owner.MySpace.YPos;
                    break;
                case "South":
                    possibility = x.MySpace.XPos == Owner.MySpace.XPos && x.MySpace.YPos < Owner.MySpace.YPos;
                    break;
                case "East":
                    possibility = x.MySpace.YPos == Owner.MySpace.YPos && x.MySpace.XPos > Owner.MySpace.XPos;
                    break;
                case "West":
                    possibility = x.MySpace.YPos == Owner.MySpace.YPos && x.MySpace.XPos < Owner.MySpace.XPos;
                    break;
            }

            return possibility && x.Controller != Owner.Controller;
        });

        if (!possibleTargets.Any())
        {
            Debug.LogError("ERROR: No orthogonal enemy targets for tongue.");
        }

        Creature trueTarget = possibleTargets.First();
        if (possibleTargets.Count() > 1)
        {
            foreach (var pos in possibleTargets)
            {
                if (GachaGrid.DistanceBetween(pos, Owner) < GachaGrid.DistanceBetween(trueTarget, Owner))
                {
                    trueTarget = pos;
                }
            }
        }

        return trueTarget;
    }

    private Func<Ability, bool> GetValidForDirection(string dir)
    {
        Func<Ability, bool> retFunc = x =>
        {
            var grid = Owner.MyGame.GameGrid;
            var startSpace = Owner.MySpace;

            GridSpace targetSpace = startSpace;
            var shouldGo = true;
            while (shouldGo)
            {
                shouldGo = false;

                //TODO: Jesus christ. Fix this.
                switch (dir)
                {
                    case "North":
                        if(targetSpace.YPos < grid.Height - 1)
                        {
                            targetSpace = grid[(targetSpace.XPos, targetSpace.YPos + 1)];
                            if(targetSpace.Occupant != null)
                            {
                                if(targetSpace.Occupant.Controller != Owner.Controller)
                                {
                                    return true;
                                }
                                return false;
                            }
                            else if (targetSpace.isBlocked)
                            {
                                return false;
                            }
                            shouldGo = true;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case "South":
                        if (targetSpace.YPos > 0)
                        {
                            targetSpace = grid[(targetSpace.XPos, targetSpace.YPos - 1)];
                            if (targetSpace.Occupant != null)
                            {
                                if (targetSpace.Occupant.Controller != Owner.Controller)
                                {
                                    return true;
                                }
                                return false;
                            }
                            else if (targetSpace.isBlocked)
                            {
                                return false;
                            }
                            shouldGo = true;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case "East":
                        if (targetSpace.XPos < grid.Width - 1)
                        {
                            targetSpace = grid[(targetSpace.XPos + 1, targetSpace.YPos)];
                            if (targetSpace.Occupant != null)
                            {
                                if (targetSpace.Occupant.Controller != Owner.Controller)
                                {
                                    return true;
                                }
                                return false;
                            }
                            else if (targetSpace.isBlocked)
                            {
                                return false;
                            }
                            shouldGo = true;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case "West":
                        if (targetSpace.XPos > 0)
                        {
                            targetSpace = grid[(targetSpace.XPos - 1, targetSpace.YPos)];
                            if (targetSpace.Occupant != null)
                            {
                                if (targetSpace.Occupant.Controller != Owner.Controller)
                                {
                                    return true;
                                }
                                return false;
                            }
                            else if (targetSpace.isBlocked)
                            {
                                return false;
                            }
                            shouldGo = true;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                }
            }

            return false;
        };

        return retFunc;
    }
}
