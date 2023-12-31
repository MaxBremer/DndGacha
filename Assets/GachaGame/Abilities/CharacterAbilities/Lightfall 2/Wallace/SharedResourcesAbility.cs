﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class SharedResourcesAbility : ActiveAbility
{
    public SharedResourcesAbility()
    {
        Name = "SharedResources";
        DisplayName = "Shared Resources";
        MaxCooldown = 1;
    }

    public override void InitAbility()
    {
        base.InitAbility();

        var firstAbilChoice = new OptionSelectChoice() { Caption = "AbilSelect1" };
        var secondAbilChoice = new OptionSelectChoice() { Caption = "AbilSelect2" };

        Func<Creature, bool> isValid = x => x.IsOnBoard && x.Controller == Owner.Controller && x.Abilities.Count > 0;

        var firstTargetChoice = new CreatureTargetChoice() { Caption = "Target1", IsValidCreature = isValid };
        firstTargetChoice.TriggerAfterChoiceMade = () =>
        {
            firstAbilChoice.Options.Clear();
            firstAbilChoice.Options.AddRange(firstTargetChoice.TargetCreature.Abilities.Select(x => x.DisplayName));
        };

        var secondTargetChoice = new CreatureTargetChoice() { Caption = "Target2", IsValidCreature = x => isValid(x) && firstTargetChoice.TargetCreature != x };
        secondTargetChoice.TriggerAfterChoiceMade = () =>
        {
            secondAbilChoice.Options.Clear();
            secondAbilChoice.Options.AddRange(secondTargetChoice.TargetCreature.Abilities.Select(x => x.DisplayName));
        };

        ChoicesNeeded.AddRange(new Choice[] { firstTargetChoice, secondTargetChoice, firstAbilChoice, secondAbilChoice });
    }

    public override void Trigger(object sender, EventArgs e)
    {
        var target1 = (ChoicesNeeded.Where(x => x.Caption == "Target1").First() as CreatureTargetChoice).TargetCreature;
        var target2 = (ChoicesNeeded.Where(x => x.Caption == "Target2").First() as CreatureTargetChoice).TargetCreature;

        var chosenAbil1 = (ChoicesNeeded.Where(x => x.Caption == "AbilSelect1").First() as OptionSelectChoice).ChosenOption;
        var chosenAbil2 = (ChoicesNeeded.Where(x => x.Caption == "AbilSelect2").First() as OptionSelectChoice).ChosenOption;

        var targetAbility1 = target1.Abilities.Where(x => x.DisplayName == chosenAbil1).First();
        var targetAbility2 = target2.Abilities.Where(x => x.DisplayName == chosenAbil2).First();

        target1.RemoveAbility(targetAbility1, false);
        target2.RemoveAbility(targetAbility2, false);
        target1.GainAbility(targetAbility2, false);
        target2.GainAbility(targetAbility1, false);

        if (AbilityRank == 2)
        {
            if(targetAbility1 is ActiveAbility active1 && active1.Cooldown > 0)
            {
                active1.CooldownTick();
            }

            if (targetAbility2 is ActiveAbility active2 && active2.Cooldown > 0)
            {
                active2.CooldownTick();
            }
        }

        if(AbilityRank < 1)
        {
            MaxCooldown++;
        }
    }

    public override void UpdateDescription()
    {
        Description = "Choose two friendly characters, then choose an ability from each. They swap these abilities." + (AbilityRank < 2 ? "" : " Reduce cooldown counters of those abilities by 1 (if they have them).") + (AbilityRank < 1 ? " Increase the cooldown of this ability by 1." : "");
    }

    public override void RankUpToOne()
    {
    }

    public override void RankUpToTwo()
    {
    }
}
