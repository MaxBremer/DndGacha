using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EasilyFrightenedAbility : AuraAbility
{
    private List<Creature> _afraidOf = new List<Creature>();
    private bool _cantActOn = false;
    private bool _cantMoveOn = false;

    public EasilyFrightenedAbility()
    {
        Name = "EasilyFrightened";
        DisplayName = "Easily Frightened";
        Description = "Cannot act or move when within range 2 of an enemy character.";
    }

    public override void AddOnboardTriggers()
    {
        base.AddOnboardTriggers();
        EventManager.StartListening(GachaEventType.CreatureEntersSpace, ConditionalTrigger, Priority);
        EventManager.StartListening(GachaEventType.CreatureLeavesSpace, OnCreatureLeaves, Priority);
        EventManager.StartListening(GachaEventType.CreatureLeavesBoard, OnCreatureLeavesBoard, Priority);
    }

    public override void RemoveOnboardTriggers()
    {
        base.RemoveOnboardTriggers();
        EventManager.StopListening(GachaEventType.CreatureEntersSpace, ConditionalTrigger, Priority);
        EventManager.StopListening(GachaEventType.CreatureLeavesSpace, OnCreatureLeaves, Priority);
        EventManager.StopListening(GachaEventType.CreatureLeavesBoard, OnCreatureLeavesBoard, Priority);
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(e is CreatureSpaceArgs spaceArgs)
        {
            if (GachaGrid.IsInRange(Owner, spaceArgs.SpaceInvolved, 2) && spaceArgs.MyCreature.Controller != Owner.Controller)
            {
                ExternalTrigger(sender, e);
            } else if(spaceArgs.MyCreature == Owner)
            {
                var enemies = Owner.MyGame.AllCreatures.Where(x => x.IsOnBoard && x.Controller != Owner.Controller && GachaGrid.IsInRange(Owner, x, 2));
                enemies.ToList().ForEach(x =>
                {
                    if (!_afraidOf.Contains(x))
                    {
                        var newArgs = spaceArgs.CreateCopy();
                        newArgs.MyCreature = x;
                        ExternalTrigger(sender, newArgs);
                    }
                });
            }
        }
    }

    // TODO: Fix many edge cases when tag given/removed by other effect.

    public override void Trigger(object sender, EventArgs e)
    {
        if (!_cantActOn)// && !Owner.HasTag(CreatureTag.CANT_ACT))
        {
            _cantActOn = true;
            Owner.GainTag(CreatureTag.CANT_ACT);
        }

        if (!_cantMoveOn)// && !Owner.HasTag(CreatureTag.CANT_MOVE))
        {
            _cantMoveOn = true;
            Owner.GainTag(CreatureTag.CANT_MOVE);
        }

        _afraidOf.Add((e as CreatureSpaceArgs).MyCreature);
    }

    private void OnCreatureLeaves(object sender, EventArgs e)
    {
        if (e is CreatureSpaceArgs spaceArgs)
        {
            if (_afraidOf.Contains(spaceArgs.MyCreature))
            {
                _afraidOf.Remove(spaceArgs.MyCreature);
                if (_afraidOf.Count == 0)
                {
                    ClearConditions();
                }
            }
            else if (spaceArgs.MyCreature == Owner)
            {
                var toRemove = new List<Creature>();
                foreach (var creat in _afraidOf)
                {
                    if (!GachaGrid.IsInRange(Owner, creat, 2))
                    {
                        toRemove.Add(creat);
                    }
                }

                toRemove.ForEach(x => _afraidOf.Remove(x));

                if(_afraidOf.Count == 0)
                {
                    ClearConditions();
                }
            }
        }
    }

    private void OnCreatureLeavesBoard(object sender, EventArgs e)
    {
        if(sender is Creature c && c == Owner)
        {
            ClearConditions();
            _afraidOf.Clear();
        }
    }

    private void ClearConditions()
    {
        if (_cantActOn)
        {
            Owner.LoseTag(CreatureTag.CANT_ACT);
            _cantActOn = false;
        }

        if (_cantMoveOn)
        {
            Owner.LoseTag(CreatureTag.CANT_MOVE);
            _cantMoveOn = false;
        }
    }
}
