using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class AuraOfNightmaresAbility : MyTurnEndPassive
{
    public AuraOfNightmaresAbility()
    {
        Name = "AuraOfNightmares";
        DisplayName = "Aura of Nightmares";
        Description = "At the end of your turn, deal 1 damage to all other characters within Range 5.";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is TurnEndArgs turnArgs && turnArgs.PlayerWhoseTurnIsEnding == Owner.Controller.MyPlayerIndex && GetCandidates().Any())
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        foreach (var creat in GetCandidates())
        {
            creat.TakeDamage(1, Owner);
        }
    }

    private IEnumerable<Creature> GetCandidates()
    {
        return Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && x != Owner && GachaGrid.IsInRange(Owner, x, 5));
    }
}
