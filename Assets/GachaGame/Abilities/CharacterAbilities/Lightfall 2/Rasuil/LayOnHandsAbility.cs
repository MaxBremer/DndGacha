using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LayOnHandsAbility : TouchRangeFriendlyOrSelfAbility
{
    private const int HEAL_AMOUNT = 10;

    public int NumUses = 0;

    public LayOnHandsAbility()
    {
        Name = "LayOnHands";
        DisplayName = "Lay on Hands";
        Description = "Heal an ally in Range 1 or this character by 10. Once this is used thrice, it cannot be used again this battle.";
        MaxCooldown = 0;
    }

    public override bool IsActivateable()
    {
        return base.IsActivateable() && NumUses < 3;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.Heal(HEAL_AMOUNT);
            NumUses++;
        }
    }
}
