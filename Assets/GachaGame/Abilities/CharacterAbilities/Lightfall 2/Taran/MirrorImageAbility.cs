using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MirrorImageAbility : RangedUnblockedPointTargetAbility
{
    public MirrorImageAbility()
    {
        Name = "MirrorImage";
        DisplayName = "Mirror Image";
        Description = "Create a 1 health copy of this character in range 1 that can move but cannot act. It appears identical to this character to your opponent, and dies when this character does. You may switch this character and the Image's positions.";
        MaxCooldown = 0;
        Range = 1;
    }

    public override void InitAbility()
    {
        base.InitAbility();
        OptionSelectChoice choice = new OptionSelectChoice() { Caption = "SwapChoice", Options = new List<string>() { "Swap Positions with Image", "Don't Swap" } };
        ChoicesNeeded.Add(choice);
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is PointTargetChoice pointChoice && pointChoice.ChoiceMade && ChoicesNeeded.Where(x => x.Caption == "SwapChoice").FirstOrDefault() is OptionSelectChoice optChoice && optChoice.ChoiceMade)
        {
            var image = Owner.CreateCopy();
            image.Health = 1;
            image.GainTag(CreatureTag.CANT_ACT);
            image.GainTag(CreatureTag.ILLUSORY);
            if (optChoice.ChosenOption == "Swap Positions with Image")
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
}
