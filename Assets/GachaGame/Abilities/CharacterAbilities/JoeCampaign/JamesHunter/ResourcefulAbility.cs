using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ResourcefulAbility : MyTurnEndPassive
{
    private int HEAL_AMOUNT = 3;

    public ResourcefulAbility()
    {
        Name = "Resourceful";
        DisplayName = "Resourceful";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.Heal(HEAL_AMOUNT);
    }

    public override void UpdateDescription()
    {
        Description = "At the end of your turn, this character restores " + HEAL_AMOUNT + " health";
    }

    public override void RankUpToOne()
    {
        HEAL_AMOUNT++;
    }

    public override void RankUpToTwo()
    {
        HEAL_AMOUNT += 2;
    }
}
