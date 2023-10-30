using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class TheCurtainsOpenAbility : RangedTargetMultipleEnemiesAbility
{
    public TheCurtainsOpenAbility()
    {
        Name = "TheCurtainsOpen";
        DisplayName = "The Curtains Open";
        Description = "Choose 3 targets in Range 2: deal 3 damage to them. If \"The Lights Dim\" was used last turn, also give them -2/-2/-2 until the end of their next turn.";
        MaxCooldown = 2;
        Range = 2;
        NumChoices = 3;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        //TODO: Do better.
        var choice1 = ChoicesNeeded[0] as CreatureTargetChoice;
        var choice2 = ChoicesNeeded[1] as CreatureTargetChoice;
        var choice3 = ChoicesNeeded[2] as CreatureTargetChoice;

        if(choice1.ChoiceMade && choice2.ChoiceMade && choice3.ChoiceMade)
        {
            bool doExtra = GameUtils.AbilWasTriggeredNTurnsAgo(1, "TheLightsDim");
            var allChoices = new CreatureTargetChoice[] { choice1, choice2, choice3 };

            foreach (var target in allChoices)
            {
                target.TargetCreature.TakeDamage(3, Owner);

                if (doExtra)
                {
                    target.TargetCreature.StatsChange(AtkChg: -2, HealthChg: -2, SpeedChg: -2);
                }
            }
        }
    }
}
