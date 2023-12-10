using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class LayOnHandsAbility : TouchRangeFriendlyOrSelfAbility
{
    private int HEAL_AMOUNT = 10;
    private int _maxNumTimes = 3;

    public int NumUses = 0;

    public LayOnHandsAbility()
    {
        Name = "LayOnHands";
        DisplayName = "Lay on Hands";
        MaxCooldown = 0;
    }

    public override bool IsActivateable()
    {
        return base.IsActivateable() && NumUses < _maxNumTimes;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.Heal(HEAL_AMOUNT);
            NumUses++;
        }
    }

    public override void UpdateDescription()
    {
        Description = "Heal an ally in Range 1 or this character by " + HEAL_AMOUNT + ". Once this is used " + _maxNumTimes + " times, it cannot be used again this battle.";
    }

    public override void RankUpToOne()
    {
        HEAL_AMOUNT += 2;
        _maxNumTimes++;
    }

    public override void RankUpToTwo()
    {
        HEAL_AMOUNT += 3;
        _maxNumTimes++;
    }
}
