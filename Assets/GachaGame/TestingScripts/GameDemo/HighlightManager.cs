using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager
{
    private GameDemo gameDemo;

    public HighlightManager(GameDemo gameDemoInstance)
    {
        gameDemo = gameDemoInstance;
    }

    public void HighlightValidMoves(Creature targetCreat)
    {
        if (targetCreat.SpeedLeft > 0)
        {
            gameDemo.curValidMoveDict = gameDemo.MyGame.GameGrid.GetValidMoves(targetCreat);
            foreach (var gSpace in gameDemo.curValidMoveDict.Keys)
            {
                gameDemo.GridObjs[gSpace].square.Highlight();
            }
        }
    }

    public void HighlightValidAttackTargets()
    {
        foreach (var target in gameDemo.ValidAttackTargets)
        {
            var targetObj = gameDemo.GetOnboardComponent(target);
            if (targetObj != null)
            {
                targetObj.HighlightAttackTarget();
            }
        }
    }

    public void HighlightCreatureAbilityTargets(Ability abil, Creature[] targets)
    {
        foreach (var creat in targets)
        {
            gameDemo.GetOnboardComponent(creat).HighlightAbilTarget();
        }
        gameDemo.ValidCreatureAbilityTargets.AddRange(targets);
        gameDemo.CurrentChoiceMakingAbility = abil;
    }

    public void HighlightPointAbilityTargets(Ability abil, GridSpace[] targets)
    {
        foreach (var square in targets)
        {
            gameDemo.GetBoardSpaceComponent(square).HighlightAbilityTarget();
        }
        gameDemo.ValidPointAbilityTargets.AddRange(targets);
        gameDemo.CurrentChoiceMakingAbility = abil;
    }

    public void ClearAttackTargetHighlights()
    {
        foreach (var target in gameDemo.ValidAttackTargets)
        {
            var onboardComp = gameDemo.GetOnboardComponent(target);
            if (onboardComp != null)
            {
                onboardComp.RevertHighlightToBase();
            }
        }
    }

    public void ClearCreatureAbilityTargetHighlights()
    {
        foreach (var creat in gameDemo.ValidCreatureAbilityTargets)
        {
            gameDemo.GetOnboardComponent(creat).RevertHighlightToBase();
        }
    }

    public void ClearPointAbilityTargetHighlights()
    {
        foreach (var space in gameDemo.ValidPointAbilityTargets)
        {
            gameDemo.ResetSquareColorToBase(space);
        }
    }
}
