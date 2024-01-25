using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class AuraOfNightmaresAbility : MyTurnEndPassive
{
    private int _dmgAmount = 1;
    private int _dmgRange = 5;

    public AuraOfNightmaresAbility()
    {
        Name = "AuraOfNightmares";
        DisplayName = "Aura of Nightmares";
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

    public override void UpdateDescription()
    {
        Description = "At the end of your turn, deal " + _dmgAmount + " damage to all other characters within Range " + _dmgRange + ".";
    }

    public override void RankUpToOne()
    {
        _dmgRange += 2;
    }

    public override void RankUpToTwo()
    {
        _dmgAmount += 2;
    }

    private IEnumerable<Creature> GetCandidates()
    {
        return Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && x != Owner && GachaGrid.IsInRange(Owner, x, 5));
    }
}
