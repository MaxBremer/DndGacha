using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SkinOfEyesAbility : MyTurnStartPassive
{
    public SkinOfEyesAbility()
    {
        Name = "SkinOfEyes";
        DisplayName = "Skin of Eyes";
        Description = "At the start of your turn, this character makes a ranged attack on each creature in Range 3.";
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
            Owner.AttackTarget(creat, true);
        }
    }

    private IEnumerable<Creature> GetCandidates()
    {
        return Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && x != Owner && GachaGrid.IsInRange(x, Owner, 3));
    }
}
