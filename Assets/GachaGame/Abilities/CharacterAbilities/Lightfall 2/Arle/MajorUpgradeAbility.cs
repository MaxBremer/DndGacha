using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MajorUpgradeAbility : TouchRangeFriendlyAbility
{
    public MajorUpgradeAbility()
    {
        Name = "MajorUpgrade";
        DisplayName = "Major Upgrade";
        Description = "Double the attack and health of an adjacent turret.";
        MaxCooldown = 3;
    }

    public override void InitAbility()
    {
        base.InitAbility();
        var targetChoice = ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() as CreatureTargetChoice;
        if (targetChoice != null)
        {
            var targetChoiceIsValid = targetChoice.IsValidCreature;
            Func<Creature, bool> newIsValid = x => targetChoiceIsValid(x) && x.CreatureTypes.Contains("Turret");
            targetChoice.IsValidCreature = newIsValid;
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.StatsChange(AtkChg: creatChoice.TargetCreature.Attack, HealthChg: creatChoice.TargetCreature.MaxHealth);
        }
    }
}
