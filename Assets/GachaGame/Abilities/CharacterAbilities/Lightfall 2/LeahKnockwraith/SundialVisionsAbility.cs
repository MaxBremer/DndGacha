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

    public override void UpdateDescription()
    {
        if(AbilityRank == 0)
        {
            Description = "Any enemy attack or enemy ability that targets this character has a 25% chance to fail when used. If this is the only character you control, this chance is 50%.";
        }else if (AbilityRank == 1)
        {
            Description = "Any enemy attack or enemy ability that targets this character has a 50% chance to fail when used.";
        }else if (AbilityRank == 2)
        {
            Description = "Any enemy attack or enemy ability that targets this character has a 50% chance to fail when used. If this is the only character you control, this chance is 75%.";
        }
    }

    public override void RankUpToOne()
    {
    }

    public override void RankUpToTwo()
    {
    }

    public override void InitAbility()
    {
        base.InitAbility();
        UpdateOdds(null, new EventArgs());
    }

    private void UpdateOdds(object sender, EventArgs e)
    {
        if(AbilityRank == 1 || (IncreaseConditionsTrue() && AbilityRank == 0) || ((!IncreaseConditionsTrue()) && AbilityRank == 2))
        {
            _oddsOfCountering = 2;
        }
        else if(AbilityRank == 0)
        {
            _oddsOfCountering = 1;
        }
        else
        {
            _oddsOfCountering = 3;
        }
    }

    private bool IncreaseConditionsTrue()
    {
        return Owner.Controller.OnBoardCreatures.Count == 1 && Owner.Controller.OnBoardCreatures.First() == Owner;
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
