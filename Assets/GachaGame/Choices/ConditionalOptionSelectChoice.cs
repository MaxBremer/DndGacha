using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ConditionalOptionSelectChoice : Choice
{
    private string _chosenOption = string.Empty;

    public ConditionalOptionSelectChoice()
    {
        Type = ChoiceType.CONDOPTIONSELECT;
    }

    public Dictionary<string, Func<Ability, bool>> ChoiceConditions = new Dictionary<string, Func<Ability, bool>>();

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
