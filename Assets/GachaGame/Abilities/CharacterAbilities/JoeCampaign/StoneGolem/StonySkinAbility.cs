using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class StonySkinAbility : BeforeDamageAbility
{
    public StonySkinAbility()
    {
        Name = "StonySkin";
        DisplayName = "Stony Skin";
        Priority = 3;
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is TakingDamageArgs && sender is Creature c && Owner.MyGame.CurrentPlayer == Owner.Controller && DoesAbilityApply(c))
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

    public override void UpdateDescription()
    {
        if (AbilityRank == 0)
        {
            Description = "Cannot be damaged on your turn.";
        }
        else if(AbilityRank == 1)
        {
            Description = "This creature and adjacent friendly creatures cannot be damaged on your turn.";
        }
        else
        {
            Description = "Friendly creatures cannot be damaged on your turn.";
        }
    }

    private bool DoesAbilityApply(Creature c)
    {
        return (AbilityRank == 0 && c == Owner) ||
            (AbilityRank == 1 && (c == Owner || (GachaGrid.IsInRange(Owner, c, 1) && Owner.Controller == c.Controller))) ||
            (AbilityRank == 2 && c.Controller == Owner.Controller);
    }
}
