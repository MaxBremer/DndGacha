using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class StrictOversightAbility : TurnStartPassive
{
    private int _dmgRange = 2;
    private int _dmgAmount = 2;
    private int _atkAmount = 1;

    public StrictOversightAbility()
    {
        Name = "StrictOversight";
        DisplayName = "Strict Oversight";
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
            creat.TakeDamage(_dmgAmount, Owner);
            creat.StatsChange(AtkChg: _atkAmount);
        }
    }

    public override void UpdateDescription()
    {
        Description = "At the start of your turn, deal " + _dmgAmount + " damage to all friendly characters within Range " + _dmgRange + ". They each gain " + _atkAmount + " attack.";
    }

    public override void RankUpToOne()
    {
        _dmgRange++;
    }

    public override void RankUpToTwo()
    {
        _dmgAmount--;
        _atkAmount++;
    }

    private IEnumerable<Creature> GetCandidates()
    {
        return Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && x != Owner && x.Controller == Owner.Controller && GachaGrid.IsInRange(x, Owner, _dmgRange));
    }
}
