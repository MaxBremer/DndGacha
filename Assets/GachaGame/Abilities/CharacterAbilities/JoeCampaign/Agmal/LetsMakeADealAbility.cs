using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class LetsMakeADealAbility : RangedActiveAbility
{
    private List<Creature> ReceivedDeal = new List<Creature>();

    public LetsMakeADealAbility()
    {
        Name = "LetsMakeADeal";
        DisplayName = "Let's make a deal...";
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

    public override void UpdateDescription()
    {
        Description = "Give a character in Range " + Range + " *Agmal's Promise*. Agmal can only use *Let's Make a Deal* on each character once.";
    }

    public override void RankUpToOne()
    {
        MaxCooldown--;
    }

    public override void RankUpToTwo()
    {
        MaxCooldown--;
    }
}
