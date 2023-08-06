using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSelectChoice : Choice
{
    private string _chosenOption = string.Empty;

    public OptionSelectChoice()
    {
        Type = ChoiceType.OPTIONSELECT;
    }

    public List<string> Options = new List<string>();

    public string ChosenOption
    {
        get => _chosenOption;
        set
        {
            _chosenOption = value;
            ChoiceMade = !string.IsNullOrEmpty(_chosenOption);
        }
    }

    public override void ClearChoice()
    {
        ChosenOption = string.Empty;
    }
}
