using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ArchenemyAbility : TargetSingleEnemyAbility
{
    private Creature _theEnemy = null;
    private int _dmgAmount = 0;

    public ArchenemyAbility()
    {
        Name = "Archenemy";
        DisplayName = "Archenemy";
        MaxCooldown = 0;
    }

    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();

        EventManager.StartListening(GachaEventType.CreatureLeavesBoard, Creature_LeavingBoard, Priority);
        EventManager.StartListening(GachaEventType.CreatureAbilitySelectingTargets, Creature_AbilityTargetsBeingSelected, Priority);
        EventManager.StartListening(GachaEventType.CreatureSelectingAttackTargets, Creature_AttackTargetsBeingSelected, Priority);
    } 

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();

        EventManager.StopListening(GachaEventType.CreatureLeavesBoard, Creature_LeavingBoard, Priority);
        EventManager.StopListening(GachaEventType.CreatureAbilitySelectingTargets, Creature_AbilityTargetsBeingSelected, Priority);
        EventManager.StopListening(GachaEventType.CreatureSelectingAttackTargets, Creature_AttackTargetsBeingSelected, Priority);
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            _theEnemy = creatChoice.TargetCreature;
        }
    }

    public override void UpdateDescription()
    {
        string midSect = (_dmgAmount > 0 ? " Deal " + _dmgAmount + " damage to it." : "");
        Description = "Choose an enemy creature." + midSect + " Both that creature and this one can only target each other with abilities and attacks. Can only have one archenemy at a time.";
    }

    public override void RankUpToOne()
    {
        _dmgAmount += 2;
    }

    public override void RankUpToTwo()
    {
        _dmgAmount += 3;
    }

    private void Creature_AbilityTargetsBeingSelected(object sender, EventArgs e)
    {
        if(_theEnemy != null && e is AbilityCreatureTargetSelectingArgs creatArgs && creatArgs.CurrentTargetsBeingConsidered != null)
        {
            var toRemove = new List<Creature>();
            if (creatArgs.AbilityMakingChoice.Owner == Owner)
            {
                foreach (var possibleCreat in creatArgs.CurrentTargetsBeingConsidered)
                {
                    if(possibleCreat != _theEnemy)
                    {
                        toRemove.Add(possibleCreat);
                    }
                }
            }
            else if (creatArgs.AbilityMakingChoice.Owner == _theEnemy)
            {
                foreach (var possibleCreat in creatArgs.CurrentTargetsBeingConsidered)
                {
                    if (possibleCreat != Owner)
                    {
                        toRemove.Add(possibleCreat);
                    }
                }
            }

            foreach (var creat in toRemove)
            {
                creatArgs.CurrentTargetsBeingConsidered.Remove(creat);
            }
        }
    }

    private void Creature_AttackTargetsBeingSelected(object sender, EventArgs e)
    {
        if(_theEnemy != null && e is BasicAttackTargetingArgs atkArgs)
        {
            var toRemove = new List<Creature>();

            if (atkArgs.CreatureAttacking == Owner)
            {
                foreach (var possibleCreat in atkArgs.ValidAttackTargets)
                {
                    if(possibleCreat != _theEnemy)
                    {
                        toRemove.Add(possibleCreat);
                    }
                }
            }
            else if (atkArgs.CreatureAttacking == _theEnemy)
            {
                foreach (var possibleCreat in atkArgs.ValidAttackTargets)
                {
                    if (possibleCreat != Owner)
                    {
                        toRemove.Add(possibleCreat);
                    }
                }
            }

            foreach (var creat in toRemove)
            {
                atkArgs.ValidAttackTargets.Remove(creat);
            }
        }
    }

    private void Creature_LeavingBoard(object sender, EventArgs e)
    {
        if (_theEnemy != null && e is CreatureDiesArgs creatArgs && (creatArgs.CreatureDied == _theEnemy || creatArgs.CreatureDied == Owner))
        {
            _theEnemy = null;
        }
    }
}
