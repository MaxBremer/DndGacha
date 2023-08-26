using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LunarVolleyAbility : RangedTargetEnemyAbility
{
    private const int DAMAGE_AMOUNT = 7;
    
    public LunarVolleyAbility()
    {
        Name = "LunarVolley";
        DisplayName = "Lunar Volley";
        Description = "Choose a target within range 4. It takes 7 damage.";
        MaxCooldown = 2;
        Range = 4;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.TakeDamage(7, Owner);
        }
    }
}
