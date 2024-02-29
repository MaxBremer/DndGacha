using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class SkinOfEyesAbility : MyTurnStartPassive
{
    private int _dmgRange = 3;
    private int _atkGain = 1;

    public SkinOfEyesAbility()
    {
        Name = "SkinOfEyes";
        DisplayName = "Skin of Eyes";
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
        if(AbilityRank >= 1)
        {
            Owner.StatsChange(AtkChg: _atkGain);
        }

        foreach (var creat in GetCandidates())
        {
            Owner.AttackTarget(creat, true);
        }
    }

    public override void UpdateDescription()
    {
        var midSection = AbilityRank < 1 ? "" : "gains " + _atkGain + " attack and ";
        Description = "At the start of your turn, this character " + midSection + "makes a ranged attack on each enemy creature in Range " + _dmgRange + ".";
    }

    public override void RankUpToOne()
    {
    }

    public override void RankUpToTwo()
    {
        _atkGain += 2;
    }

    private IEnumerable<Creature> GetCandidates()
    {
        return Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && x.Controller != Owner.Controller && GachaGrid.IsInRange(x, Owner, _dmgRange));
    }
}
