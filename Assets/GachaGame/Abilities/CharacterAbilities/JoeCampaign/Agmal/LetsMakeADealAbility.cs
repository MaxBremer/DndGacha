using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LetsMakeADealAbility : ActiveAbility
{
    private List<Creature> ReceivedDeal = new List<Creature>();
    public int Range;

    public LetsMakeADealAbility()
    {
        Name = "LetsMakeADeal";
        DisplayName = "Let's make a deal...";
        Description = "Give a character in Range 4 *Agmal's Promise*. Agmal can only use *Let's Make a Deal* on each character once.";
        MaxCooldown = 2;
        Range = 4;
    }

    public override void InitAbility()
    {
        base.InitAbility();

        Func<Creature, bool> isValid = x => x.IsOnBoard && x != Owner && GachaGrid.IsInRange(x, Owner, Range) && (!ReceivedDeal.Contains(x));
        ChoicesNeeded.Add(new CreatureTargetChoice() { Caption = "Target", IsValidCreature = isValid });
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            ReceivedDeal.Add(creatChoice.TargetCreature);
            var promise = new AgmalsPromiseAbility();
            promise.PromiseGiver = Owner;
            creatChoice.TargetCreature.GainAbility(promise, true);
        }
    }
}
