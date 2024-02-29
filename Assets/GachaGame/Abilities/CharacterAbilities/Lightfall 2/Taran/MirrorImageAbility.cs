using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class MirrorImageAbility : RangedUnblockedPointTargetAbility
{
    private int _healthAmt = 1;

    public MirrorImageAbility()
    {
        Name = "MirrorImage";
        DisplayName = "Mirror Image";
        MaxCooldown = 0;
        Range = 1;
    }

    public override void InitAbility()
    {
        base.InitAbility();
        OptionSelectChoice choice = new OptionSelectChoice(new List<string>() { "Swap Positions with Image", "Don't Swap" }) { Caption = "SwapChoice" };
        ChoicesNeeded.Add(choice);
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade && ChoicesNeeded.Where(x => x.Caption == "SwapChoice").FirstOrDefault() is OptionSelectChoice optChoice && optChoice.ChoiceMade)
        {
            var image = Owner.CreateCopy();

            if(AbilityRank < 2)
            {
                image.Health = 1;
            }

            image.GainTag(CreatureTag.CANT_ACT);
            image.GainTag(CreatureTag.ILLUSORY);
            if (optChoice.ChosenOptionString == "Swap Positions with Image")
            {
                var origSpace = Owner.MySpace;
                Owner.MyGame.GameGrid.TeleportTo(Owner, pointChoice.TargetSpace);
                Owner.MyGame.SummonCreature(image, origSpace);
            }
            else
            {
                Owner.MyGame.SummonCreature(image, pointChoice.TargetSpace);
            }
        }
    }

    public override void UpdateDescription()
    {
        Description = "Create a " + (AbilityRank < 2 ? "1 health copy" : "") + " of this character in range " + Range + " that can move but cannot act. It appears identical to this character to your opponent, and dies when this character does. You may switch this character and the Image's positions.";
    }

    public override void RankUpToOne()
    {
        Range++;
    }

    public override void RankUpToTwo()
    {
    }
}
