using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BanishAbility : RangedTargetAbility
{
    public BanishAbility()
    {
        Name = "Banish";
        DisplayName = "Banish";
        Description = "Choose a creature below 10 health within range 2. Return them to their owner's reserve.";
        MaxCooldown = 3;
        Range = 2;
    }

    public override void InitAbility()
    {
        base.InitAbility();

        var choice = ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault();
        if(choice != null && choice is CreatureTargetChoice creatChoice)
        {
            Func<Creature, bool> newIsValid = x => x.IsOnBoard && x != Owner && GachaGrid.IsInRange(Owner, x, Range) && x.Health < 10;
            creatChoice.IsValidCreature = newIsValid;
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.Controller.PutInReserve(creatChoice.TargetCreature);
        }
    }
}
