using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ResourcefulAbility : MyTurnEndPassive
{
    private const int HEAL_AMOUNT = 4;

    public ResourcefulAbility()
    {
        Name = "Resourceful";
        DisplayName = "Resourceful";
        Description = "At the end of your turn, this character restores " + HEAL_AMOUNT + " health";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.Heal(HEAL_AMOUNT);
    }
}
