using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class BanishAbility : RangedTargetAbility
{
    private int _healthCeil = 10;

    public BanishAbility()
    {
        Name = "Banish";
        DisplayName = "Banish";
        MaxCooldown = 3;
        Range = 2;
    }

    public override void InitAbility()
    {
        base.InitAbility();

        var choice = ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault();
        if(choice != null && choice is CreatureTargetChoice creatChoice)
        {
            Func<Creature, bool> newIsValid = x => x.IsOnBoard && x != Owner && GachaGrid.IsInRange(Owner, x, Range) && x.Health < _healthCeil;
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

    public override void UpdateDescription()
    {
        Description = "Choose a creature below " + _healthCeil + " health within range " + Range + ". Return them to their owner's reserve.";
    }

    public override void RankUpToTwo()
    {
        _healthCeil += 5;
    }
}
