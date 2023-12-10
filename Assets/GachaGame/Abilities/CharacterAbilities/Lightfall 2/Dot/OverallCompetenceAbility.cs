using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OverallCompetenceAbility : WhenImCalledAbility
{
    public OverallCompetenceAbility()
    {
        Name = "OverallCompetence";
        DisplayName = "Overall... Competence";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        base.Trigger(sender, e);
        var r = new System.Random();
        var keyList = AbilityDatabase.ValidRandomAbilities;
        var key = keyList[r.Next(keyList.Count)];
        var newAbil = (Ability)Activator.CreateInstance(AbilityDatabase.AbilityDictionary[key]);

        Owner.GainAbility(newAbil, true, AbilityRank);
        Owner.RemoveAbility(this);
    }

    public override void UpdateDescription()
    {
        Description = "When this character is called, replace this ability with a random rank " + AbilityRank + " ability.";
    }

    public override void RankUpToOne()
    {
        
    }

    public override void RankUpToTwo()
    {
        
    }
}
