using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class LunarVolleyAbility : RangedTargetEnemyAbility
{
    private const int DAMAGE_AMOUNT = 7;
    
    public LunarVolleyAbility()
    {
        Name = "LunarVolley";
        DisplayName = "Lunar Volley";
        MaxCooldown = 2;
        Range = 4;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.TakeDamage(DAMAGE_AMOUNT, Owner);
        }
    }

    public override void UpdateDescription()
    {
        Description = "Choose a target within range " + Range + ". It takes " + DAMAGE_AMOUNT + " damage.";
    }

    public override void RankUpToTwo()
    {
        Range++;
        MaxCooldown--;
    }
}
