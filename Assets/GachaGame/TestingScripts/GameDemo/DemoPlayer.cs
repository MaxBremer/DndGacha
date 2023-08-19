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

    public void ClearChoices()
    {
        foreach (var choice in ChoicesToMake)
        {
            choice.ClearChoice();
        }

        ChoicesToMake.Clear();
        CurrentTargetAbility = null;
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
                    MyGameDemo.MyHighlightManager.HighlightCreatureAbilityTargets(CurrentTargetAbility, validTargetCreats);
                    MyGameDemo.MySelectState = GameDemoSelectState.TARGETSELECT;
                }
                break;

            case ChoiceType.POINTTARGET:
                var currentTargetChoice = CurrentTargetChoice as PointTargetChoice;
                GridSpace[] validTargetSpaces = ChoiceManager.AllValidChoicesPoint(currentTargetChoice);

                if (validTargetSpaces.Length > 0)
                {
                    MyGameDemo.MyHighlightManager.HighlightPointAbilityTargets(CurrentTargetAbility, validTargetSpaces);
                    MyGameDemo.MySelectState = GameDemoSelectState.TARGETSELECT;
                }
                break;

            case ChoiceType.OPTIONSELECT:
                var currentOptionChoice = CurrentTargetChoice as OptionSelectChoice;
                HandleOptionSelectChoice(currentOptionChoice);
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

    private void HandleOptionSelectChoice(OptionSelectChoice choice)
    {
        MyGameDemo.DisplayOptionButtons(choice.Options, (selectedOption) =>
        {
            choice.ChosenOption = selectedOption;

            // Call a function in GameDemo to remove the displayed buttons
            MyGameDemo.ClearOptionAbilityTargets(false);

            // Call the next choice
            PotentialNextChoice();
        });
        MyGameDemo.MySelectState = GameDemoSelectState.TARGETSELECT;
    }
}
