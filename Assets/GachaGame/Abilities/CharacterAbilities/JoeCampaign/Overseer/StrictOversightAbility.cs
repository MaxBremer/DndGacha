using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class StrictOversightAbility : TurnStartPassive
{
    public StrictOversightAbility()
    {
        Name = "StrictOversight";
        DisplayName = "Strict Oversight";
        Description = "At the start of your turn, deal 2 damage to all friendly characters within Range 2. They each gain 1 attack.";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is TurnStartArgs turnArgs && turnArgs.PlayerWhoseTurnIsStarting == Owner.Controller.MyPlayerIndex && GetCandidates().Any())
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        foreach (var creat in GetCandidates())
        {
            creat.TakeDamage(2, Owner);
            creat.StatsChange(AtkChg: 1);
        }
    }

    private IEnumerable<Creature> GetCandidates()
    {
        return Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && x != Owner && x.Controller == Owner.Controller && GachaGrid.IsInRange(x, Owner, 2));
    }
}
