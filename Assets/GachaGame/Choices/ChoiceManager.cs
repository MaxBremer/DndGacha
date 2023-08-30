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

    public static Ability AbilityPending { get => _abilityPending; }

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
            if(ValidChoicesExist(abil.ChoicesNeeded, abil))
            {
                abil.Owner.Controller.StartMakingChoices(abil);
            }
            else
            {
                CancelActiveChoiceAbility(_abilityPending);
            }
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
            if(_abilityPending == null)
            {
                Debug.Log("Ability pending was null");
            }
            Debug.Log("Abil pending is " + _abilityPending.Name);
            Debug.Log("Called choices made on " + abil.Name);
            Debug.LogError("ERROR: Ability triggered 'choices made' in the wrong order!");
            return;
        }

        if (!_abilityPending.AllChoicesMade())
        {
            Debug.Log("Abil pending choices was " + _abilityPending.Name);
            Debug.LogError("ERROR: NOT ALL CHOICES MADE BEFORE 'choices made' WAS TRIGGERED");
            return;
        }

        //Is it truly always triggered by itself here??
        _abilityPending.ExternalTrigger(_abilityPending, new System.EventArgs());

        PotentiallyTriggerNextAbilInQueue();
    }

    public static void ChooseRandomlyAndTrigger(Ability abil, object sender)
    {
        MakeChoicesRandomly(abil.ChoicesNeeded, abil);
        abil.ExternalTrigger(sender, new System.EventArgs());
    }

    public static void MakeChoicesRandomly(IEnumerable<Choice> toMake, Ability abilOfChoices)
    {
        foreach (var choice in toMake)
        {
            if(ValidChoiceExists(choice, abilOfChoices))
            {
                MakeChoiceRandomly(choice, abilOfChoices);
            }
        }
    }

    public static void MakeChoiceRandomly(Choice choice, Ability abilOfChoice)
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

            case ChoiceType.CONDOPTIONSELECT:
                if (choice is ConditionalOptionSelectChoice condChoice)
                {
                    var validOptions = condChoice.ChoiceConditions.Keys.Where(x => condChoice.ChoiceConditions[x](abilOfChoice)).ToList();
                    condChoice.ChosenOption = validOptions[r.Next(validOptions.Count)];
                }
                break;
            default:
                break;
        }
    }

    public static void CancelActiveChoiceAbility(Ability abil)
    {
        if(_abilityPending == null || _abilityPending != abil)
        {
            return;
        }
        PotentiallyTriggerNextAbilInQueue();
    }

    public static bool ValidChoicesExist(IEnumerable<Choice> choices, Ability abilOfChoices)
    {
        foreach (var choice in choices)
        {
            if (!ValidChoiceExists(choice, abilOfChoices))
            {
                return false;
            }
        }

        return true;
    }

    public static bool ValidChoiceExists(Choice choice, Ability abilOfChoice)
    {
        if (!choice.ConditionOfPresentation())
        {
            return true;
        }
        switch (choice.Type)
        {
            case ChoiceType.CREATURETARGET:
                return CurrentGame.AllCreatures.Where(x => ((CreatureTargetChoice)choice).IsValidCreature(x)).Any();
            case ChoiceType.POINTTARGET:
                return CurrentGame.GameGrid.GetAllGridSquares().Where(x => ((PointTargetChoice)choice).IsValidSpace(x)).Any();
            case ChoiceType.OPTIONSELECT:
                return true;
            case ChoiceType.CONDOPTIONSELECT:
                return ((ConditionalOptionSelectChoice)choice).ChoiceConditions.Values.Where(x => x(abilOfChoice)).Any();
            case ChoiceType.NONE:
            default:
                return false;
        }
    }

    public static bool NumValidChoicesExist(Choice choice, Ability abilOfChoice, int numReq)
    {
        if (!choice.ConditionOfPresentation())
        {
            return true;
        }
        switch (choice.Type)
        {
            case ChoiceType.CREATURETARGET:
                return CurrentGame.AllCreatures.Where(x => ((CreatureTargetChoice)choice).IsValidCreature(x)).Count() >= numReq;
            case ChoiceType.POINTTARGET:
                return CurrentGame.GameGrid.GetAllGridSquares().Where(x => ((PointTargetChoice)choice).IsValidSpace(x)).Count() >= numReq;
            case ChoiceType.OPTIONSELECT:
                return true;
            case ChoiceType.CONDOPTIONSELECT:
                return ((ConditionalOptionSelectChoice)choice).ChoiceConditions.Values.Where(x => x(abilOfChoice)).Count() >= numReq;
            case ChoiceType.NONE:
            default:
                return false;
        }
    }

    public static Creature[] AllValidChoicesCreature(CreatureTargetChoice choice)
    {
        return CurrentGame.AllCreatures.Where(x => choice.IsValidCreature(x)).ToArray();
    }

    public static GridSpace[] AllValidChoicesPoint(PointTargetChoice choice)
    {
        return CurrentGame.GameGrid.GetAllGridSquares().Where(x => choice.IsValidSpace(x)).ToArray();
    }

    private static void PotentiallyTriggerNextAbilInQueue()
    {
        _abilityPending = null;

        if (_waitlistAbilities.Count > 0)
        {
            var nextAbil = _waitlistAbilities.Dequeue();
            TriggerBasicPlayerDecision(nextAbil);
        }
    }
}