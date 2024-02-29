using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSelectChoice : Choice
{
    //private string _chosenOption = string.Empty;
    private ChoiceOption _chosenOption = null;

    public OptionSelectChoice()
    {
        Type = ChoiceType.OPTIONSELECT;
    }

    public OptionSelectChoice(IEnumerable<string> choices)
    {
        Type = ChoiceType.OPTIONSELECT;

        foreach (var choice in choices)
        {
            Options.Add(new ChoiceOption() { OptionName = choice, });
        }

    }

    public OptionSelectChoice(IEnumerable<ChoiceOption> choices)
    {
        Type = ChoiceType.OPTIONSELECT;

        Options.AddRange(choices);
    }

    public List<ChoiceOption> Options = new List<ChoiceOption>();

    public string ChosenOptionString
    {
        get => _chosenOption.OptionName;
    }

    public ChoiceOption ChosenOption
    {
        get => _chosenOption;
        set
        {
            _chosenOption = value;
            //Huh, first time I've ever had to cast null. It's cause of the custom != operator that ChoiceOptions have, which takes priority since null on its own could be a string.
            ChoiceMade = _chosenOption != (ChoiceOption)null;
            if (ChoiceMade)
            {
                TriggerAfterChoiceMade();
            }
        }
    }

    public override void ClearChoice()
    {
        ChosenOption = null;
    }
}
