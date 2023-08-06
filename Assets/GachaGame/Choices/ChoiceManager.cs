using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ChoiceManager
{
    private static Ability _abilityPending;
    private static Queue<Ability> _waitlistAbilities = new Queue<Ability>();
    private static List<Choice> _choicesForAbilityPending = new List<Choice>();

    public static Game CurrentGame { get; set; }

    public static void Reset(Game game)
    {
        CurrentGame = game;
        _abilityPending = null;
        _choicesForAbilityPending.Clear();
    }

    public static void TriggerBasicPlayerDecision(Ability abil)
    {
        if (_abilityPending == null)
        {
            _abilityPending = abil;
            abil.Owner.Controller.StartMakingChoices(abil);
        }
        else
        {
            _waitlistAbilities.Enqueue(abil);
        }
    }

    public static void AbilityChoicesMade(Ability abil)
    {
        if (_abilityPending != abil)
        {
            Debug.LogError("ERROR: Ability triggered 'choices made' in the wrong order!");
            return;
        }

        if (!_abilityPending.AllChoicesMade())
        {
            Debug.LogError("ERROR: NOT ALL CHOICES MADE BEFORE 'choices made' WAS TRIGGERED");
            return;
        }

        //Is it truly always triggered by itself here??
        _abilityPending.ExternalTrigger(_abilityPending, new System.EventArgs());
        _abilityPending = null;

        if(_waitlistAbilities.Count > 0)
        {
            var nextAbil = _waitlistAbilities.Dequeue();
            TriggerBasicPlayerDecision(nextAbil);
        }
    }

    public static void ChooseRandomlyAndTrigger(Ability abil, object sender)
    {
        MakeChoicesRandomly(abil.ChoicesNeeded);
        abil.ExternalTrigger(sender, new System.EventArgs());
    }

    public static void MakeChoicesRandomly(IEnumerable<Choice> toMake)
    {
        foreach (var choice in toMake)
        {
            MakeChoiceRandomly(choice);
        }
    }

    public static void MakeChoiceRandomly(Choice choice)
    {
        var r = new System.Random();
        switch (choice.Type)
        {
            case ChoiceType.NONE:
                break;
            case ChoiceType.CREATURETARGET:
                if (choice is CreatureTargetChoice creatChoice)
                {
                    var possibleChoices = new List<Creature>();
                    possibleChoices.AddRange(CurrentGame.AllCreatures.Where(x => creatChoice.IsValidCreature(x)));
                    creatChoice.TargetCreature = possibleChoices[r.Next(possibleChoices.Count)];
                }
                break;
            case ChoiceType.POINTTARGET:
                if (choice is PointTargetChoice pointChoice)
                {
                    var possibleSpaces = new List<GridSpace>();
                    possibleSpaces.AddRange(CurrentGame.GameGrid.GetAllGridSquares().Where(x => pointChoice.IsValidSpace(x)));
                    pointChoice.TargetSpace = possibleSpaces[r.Next(possibleSpaces.Count)];
                }
                break;
            case ChoiceType.OPTIONSELECT:
                if (choice is OptionSelectChoice optChoice)
                {
                    optChoice.ChosenOption = optChoice.Options[r.Next(optChoice.Options.Count)];
                }
                break;
            default:
                break;
        }
    }

    public static bool ValidChoicesExist(IEnumerable<Choice> choices)
    {
        foreach (var choice in choices)
        {
            if (!ValidChoiceExists(choice))
            {
                return false;
            }
        }

        return true;
    }

    public static bool ValidChoiceExists(Choice choice)
    {
        switch (choice.Type)
        {
            case ChoiceType.CREATURETARGET:
                return CurrentGame.AllCreatures.Where(x => ((CreatureTargetChoice)choice).IsValidCreature(x)).Any();
            case ChoiceType.POINTTARGET:
                return CurrentGame.GameGrid.GetAllGridSquares().Where(x => ((PointTargetChoice)choice).IsValidSpace(x)).Any();
            case ChoiceType.OPTIONSELECT:
                return true;
            case ChoiceType.NONE:
            default:
                return false;
        }
    }

    public static bool NumValidChoicesExist(Choice choice, int numReq)
    {
        switch (choice.Type)
        {
            case ChoiceType.CREATURETARGET:
                return CurrentGame.AllCreatures.Where(x => ((CreatureTargetChoice)choice).IsValidCreature(x)).Count() >= numReq;
            case ChoiceType.POINTTARGET:
                return CurrentGame.GameGrid.GetAllGridSquares().Where(x => ((PointTargetChoice)choice).IsValidSpace(x)).Count() >= numReq;
            case ChoiceType.OPTIONSELECT:
                return true;
            case ChoiceType.NONE:
            default:
                return false;
        }
    }
}