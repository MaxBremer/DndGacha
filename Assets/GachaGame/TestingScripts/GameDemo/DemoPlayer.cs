using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemoPlayer : Player
{
    private Ability CurrentTargetAbility = null;
    private List<Choice> ChoicesToMake = new List<Choice>();
    public Choice CurrentTargetChoice = null;

    public GameDemo MyGameDemo;

    public DemoPlayer(PlayerArgs args) : base(args) { }
    public override void StartMakingChoices(Ability abil)
    {
        ChoicesToMake.AddRange(abil.ChoicesNeeded);
        CurrentTargetAbility = abil;
        PotentialNextChoice();
        // Once all choices have been handled, call AbilityChoicesMade
        //ChoiceManager.AbilityChoicesMade(abil);
    }

    public void SelectCreatureTarget(Creature clickedCreature)
    {
        if (CurrentTargetChoice != null && CurrentTargetChoice is CreatureTargetChoice creatChoice)
        {
            creatChoice.TargetCreature = clickedCreature;
            MyGameDemo.ClearCreatureAbilityTargets(false);
            PotentialNextChoice();
        }
    }

    public void SelectPointTarget(GridSpace clickedSpace)
    {
        if (CurrentTargetChoice != null && CurrentTargetChoice is PointTargetChoice pointChoice)
        {
            pointChoice.TargetSpace = clickedSpace;
            MyGameDemo.ClearPointAbilityTargets(false);
            PotentialNextChoice();
        }
    }

    private void MakeNextChoice(Choice target)
    {
        if(target == null)
        {
            Debug.LogWarning("TRIED TO MAKE NULL CHOICE");
            return;
        }
        CurrentTargetChoice = target;

        switch (CurrentTargetChoice.Type)
        {
            case ChoiceType.CREATURETARGET:
                var currentCreatureChoice = CurrentTargetChoice as CreatureTargetChoice;
                Creature[] validTargetCreats = ChoiceManager.AllValidChoicesCreature(currentCreatureChoice);

                if (validTargetCreats.Length > 0)
                {
                    MyGameDemo.HighlightCreatureAbilityTargets(CurrentTargetAbility, validTargetCreats);
                }
                break;

            case ChoiceType.POINTTARGET:
                Debug.Log("Point choice");
                var currentTargetChoice = CurrentTargetChoice as PointTargetChoice;
                GridSpace[] validTargetSpaces = ChoiceManager.AllValidChoicesPoint(currentTargetChoice);

                if (validTargetSpaces.Length > 0)
                {
                    Debug.Log("Highlight available");
                    MyGameDemo.HighlightPointAbilityTargets(CurrentTargetAbility, validTargetSpaces);
                }
                break;

            case ChoiceType.OPTIONSELECT:
                // Handle the OPTIONSELECT choice type
                // TODO: Implement the specific logic for this choice type
                Debug.Log("Handling OPTIONSELECT choice");
                break;

            default:
                Debug.LogError("Unknown choice type encountered");
                break;
        }
    }

    private void PotentialNextChoice()
    {
        var nextChoice = GetNextUnmadeChoice();
        if(nextChoice != null)
        {
            Debug.Log("Making next choice");
            MakeNextChoice(nextChoice);
        }
        else
        {
            ChoiceManager.AbilityChoicesMade(CurrentTargetAbility);
            MyGameDemo.PotentialDeselect();
            ChoicesToMake.Clear();
            CurrentTargetAbility = null;
        }
    }

    private Choice GetNextUnmadeChoice()
    {
        Choice ret = null;

        foreach (var choice in ChoicesToMake)
        {
            if (!choice.ChoiceMade)
            {
                ret = choice;
                break;
            }
        }

        return ret;
    }
}
