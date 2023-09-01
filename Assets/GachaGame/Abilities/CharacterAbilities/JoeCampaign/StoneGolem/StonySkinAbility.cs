using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StonySkinAbility : BeforeDamageAbility
{
    public StonySkinAbility()
    {
        Name = "StonySkin";
        DisplayName = "Stony Skin";
        Description = "Cannot be damaged on your turn.";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is TakingDamageArgs && sender is Creature c && Owner == c && Owner.MyGame.CurrentPlayer == Owner.Controller)
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(e is TakingDamageArgs dmgArgs)
        {
            dmgArgs.DamageAmount = 0;
        }
    }
}
