using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallCompetenceAbility : WhenImCalledAbility
{
    public OverallCompetenceAbility()
    {
        Name = "OverallCompetence";
        DisplayName = "Overall... Competence";
        Description = "When this character is called, replace this ability with a random ability.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        base.Trigger(sender, e);
        var r = new System.Random();
        var keyList = AbilityDatabase.ValidRandomAbilities;
        var key = keyList[r.Next(keyList.Count)];
        var newAbil = (Ability)Activator.CreateInstance(AbilityDatabase.AbilityDictionary[key]);

        Owner.GainAbility(newAbil, true);
        Owner.RemoveAbility(this);
    }
}
