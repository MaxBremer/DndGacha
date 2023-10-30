using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class TwinshotAbility : BeforeMyAbilityActivatesAbility
{
    public TwinshotAbility()
    {
        Name = "Twinshot";
        DisplayName = "Twinshot";
        Description = "This characters active abilities activate twice if possible.";
        Priority = -1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is AbilityActivateArgs abilArgs)
        {
            abilArgs.NumActivations = Math.Max(abilArgs.NumActivations, 2);
        }
    }
}
