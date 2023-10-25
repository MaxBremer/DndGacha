using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SurpriseAssistanceAbility : TargetFriendlyOrSelfAbility
{
    private int _timesUntilReplacement = 5;

    public SurpriseAssistanceAbility()
    {
        Name = "SurpriseAssistance";
        DisplayName = "Surprise Assistance";
        Description = "Choose a friendly character. Reduce a random one of their nonzero cooldown counters by 1. Once this is used 5 times, replace it with Might as well be dead.";
        MaxCooldown = 0;
    }

    public override void InitAbility()
    {
        base.InitAbility();
        var targetChoice = ChoicesNeeded.Where(x => x.Caption == "Target").First() as CreatureTargetChoice;
        var isValid = targetChoice.IsValidCreature;
        targetChoice.IsValidCreature = x => isValid(x) && x.Abilities.Where(y => y is ActiveAbility active && active.Cooldown > 0).Any();
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            var abilOptions = creatChoice.TargetCreature.Abilities.Where(y => y is ActiveAbility active && active.Cooldown > 0).ToArray();
            var r = new Random();
            var chosen = abilOptions[r.Next(abilOptions.Length)];
            (chosen as ActiveAbility).CooldownTick();

            _timesUntilReplacement = Math.Max(_timesUntilReplacement - 1, 0);
            if (_timesUntilReplacement < 1)
            {
                var targetCreat = Owner;
                targetCreat.RemoveAbility(this);
                targetCreat.GainAbility(new MightAsWellBeDeadAbility());
            }
        }
    }
}
