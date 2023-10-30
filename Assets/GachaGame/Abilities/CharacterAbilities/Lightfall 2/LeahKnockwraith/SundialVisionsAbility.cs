using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class SundialVisionsAbility : PassiveAbility
{
    // 1 is 25%, 2 is 50%
    private int _oddsOfCountering = 1;

    public SundialVisionsAbility()
    {
        Name = "SundialVisions";
        DisplayName = "Sundial Visions";
        Description = "Any enemy attack or enemy ability that targets this character has a 25% chance to fail when used. If this is the only character you control, this chance is 50%.";
        Priority = 5;
    }

    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();

        EventManager.StartListening(GachaEventType.BeforeAbilityTrigger, ActiveAbility_Activated, Priority);
        EventManager.StartListening(GachaEventType.BeforeAttack, Attack_TargetSelected, Priority);
        EventManager.StartListening(GachaEventType.CreatureLeavesBoard, UpdateOdds, Priority);
        EventManager.StartListening(GachaEventType.CreatureSummoned, UpdateOdds, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();

        EventManager.StopListening(GachaEventType.BeforeAbilityTrigger, ActiveAbility_Activated, Priority);
        EventManager.StopListening(GachaEventType.BeforeAttack, Attack_TargetSelected, Priority);
        EventManager.StopListening(GachaEventType.CreatureLeavesBoard, UpdateOdds, Priority);
        EventManager.StopListening(GachaEventType.CreatureSummoned, UpdateOdds, Priority);
    }

    private void UpdateOdds(object sender, EventArgs e)
    {
        if(Owner.Controller.OnBoardCreatures.Count == 1 && Owner.Controller.OnBoardCreatures.First() == Owner)
        {
            _oddsOfCountering = 2;
        }
        else
        {
            _oddsOfCountering = 1;
        }
    }

    private void ActiveAbility_Activated(object sender, EventArgs e)
    {
        if(e is BeforeAbilityTriggerArgs abilArgs && abilArgs.AbilTriggering is ActiveAbility activeAbil && activeAbil.Owner.Controller != Owner.Controller && activeAbil.ChoicesNeeded.Where(x => x is CreatureTargetChoice creatChoice && creatChoice.TargetCreature == Owner).Any())
        {
            var r = new Random();
            if (r.Next(4) < _oddsOfCountering)
            {
                abilArgs.Countered = true;
            }
        }
    }

    private void Attack_TargetSelected(object sender, EventArgs e)
    {
        if(e is AttackArgs atkArgs && atkArgs.Target == Owner && sender is Creature c && c.Controller != Owner.Controller)
        {
            var r = new Random();
            if (r.Next(4) < _oddsOfCountering)
            {
                atkArgs.Target = null;
            }
        }
    }
}
