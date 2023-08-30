using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChoiceType
{
    NONE,
    CREATURETARGET,
    POINTTARGET,
    OPTIONSELECT,
    CONDOPTIONSELECT,
}

public abstract class Choice
{
    public ChoiceType Type;
    public bool ChoiceMade = false;
    public string Caption;

    public abstract void ClearChoice();

    public Func<bool> ConditionOfPresentation = () => true; 
}
