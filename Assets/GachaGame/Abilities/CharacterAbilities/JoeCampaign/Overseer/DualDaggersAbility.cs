using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DualDaggersAbility : RangedTargetMultipleEnemiesAbility
{
    public DualDaggersAbility()
    {
        Name = "DualDaggers";
        DisplayName = "Dual Daggers";
        Description = "Attack twice.";
        MaxCooldown = 0;
        Range = 1;
        NumChoices = 2;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        foreach (var choice in ChoicesNeeded)
        {
            if(choice is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
            {
                Owner.AttackTarget(creatChoice.TargetCreature);
            }
        }
    }
}
