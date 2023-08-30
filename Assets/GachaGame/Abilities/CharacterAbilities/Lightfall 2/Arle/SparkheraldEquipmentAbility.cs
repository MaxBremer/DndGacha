using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SparkheraldEquipmentAbility : TouchRangeFriendlyOrSelfAbility
{
    private const int ATK_INCREASE_AMOUNT = 2;

    private List<Creature> _thoseWhoHaveReceived = new List<Creature>();

    public SparkheraldEquipmentAbility()
    {
        Name = "SparkheraldEquipment";
        DisplayName = "Sparkherald Equipment";
        Description = "Give an adjacent ally or this character +" + ATK_INCREASE_AMOUNT + " attack. Each character can only receive this boost once";
        MaxCooldown = 0;
    }

    public override void InitAbility()
    {
        base.InitAbility();
        var targetChoice = ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() as CreatureTargetChoice;
        if(targetChoice != null)
        {
            var targetChoiceIsValid = targetChoice.IsValidCreature;
            Func<Creature, bool> newIsValid = x => targetChoiceIsValid(x) && !_thoseWhoHaveReceived.Contains(x);
            targetChoice.IsValidCreature = newIsValid;
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            _thoseWhoHaveReceived.Add(creatChoice.TargetCreature);
            creatChoice.TargetCreature.StatsChange(AtkChg: ATK_INCREASE_AMOUNT);
        }
    }
}
