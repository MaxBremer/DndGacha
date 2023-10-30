using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ShowtimeAbility : ActiveAbility
{
    private const int AMOUNT_ATTACK_INCREASES = 6;

    private bool _firstNextTurn;
    private int _gainedImmune = 0;

    public ShowtimeAbility()
    {
        Name = "Showtime";
        DisplayName = "Showtime";
        Description = "Until the end of your next turn, increase this characters attack by " + AMOUNT_ATTACK_INCREASES + ". If \"The Curtains Open\" was used last turn, also become Immune for this period.";
        MaxCooldown = 3;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        Owner.StatsChange(AtkChg: AMOUNT_ATTACK_INCREASES);
        _firstNextTurn = true;
        EventManager.StartListening(GachaEventType.EndOfTurn, DecreaseAttack, Priority);

        if(GameUtils.AbilWasTriggeredNTurnsAgo(1, "TheCurtainsOpen"))
        {
            Owner.GainTag(CreatureTag.IMMUNE);
            _gainedImmune++;
        }
    }

    private void DecreaseAttack(object sender, EventArgs e)
    {
        if (e is TurnEndArgs turnArgs && turnArgs.PlayerWhoseTurnIsEnding == Owner.Controller.MyPlayerIndex)
        {
            if (_firstNextTurn)
            {
                _firstNextTurn = false;
            }
            else
            {
                Owner.StatsChange(AtkChg: -1 * AMOUNT_ATTACK_INCREASES);
                if(_gainedImmune > 0)
                {
                    Owner.LoseTag(CreatureTag.IMMUNE);
                    _gainedImmune--;
                }
                EventManager.StopListening(GachaEventType.EndOfTurn, DecreaseAttack, Priority);
            }
        }
    }
}
