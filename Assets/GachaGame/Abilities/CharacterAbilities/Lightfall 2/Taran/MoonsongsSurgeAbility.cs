using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class MoonsongsSurgeAbility : ActiveAbility
{
    private ActiveAbility _mirrorAbil = null;
    private ActiveAbility _volleyAbil = null;

    public MoonsongsSurgeAbility()
    {
        Name = "MoonsongsSurge";
        DisplayName = "Moonsong's Surge";
        MaxCooldown = 4;
    }

    public override void InitAbility()
    {
        base.InitAbility();
        _mirrorAbil = Owner.Abilities.Where(x => x.Name == "MirrorImage").FirstOrDefault() as ActiveAbility;
        _volleyAbil = Owner.Abilities.Where(x => x.Name == "LunarVolley").FirstOrDefault() as ActiveAbility;
        Func<Ability, bool> mirrorIsValid;
        Func<Ability, bool> volleyIsValid;

        if(_mirrorAbil == null)
        {
            mirrorIsValid = _ => false;
        }
        else
        {
            mirrorIsValid = x => ChoiceManager.ValidChoiceExists(_mirrorAbil.ChoicesNeeded.First(), _mirrorAbil);
        }

        if(_volleyAbil == null)
        {
            volleyIsValid = _ => false;
        }
        else
        {
            volleyIsValid = x => ChoiceManager.ValidChoiceExists(_volleyAbil.ChoicesNeeded.First(), _volleyAbil);
        }

        //TODO: When abilities of owner change, update choice to reflect this? I.e. update isvalid functions in cond option.

        var chooseWhichToActivate = new ConditionalOptionSelectChoice() { Caption = "Options" };
        chooseWhichToActivate.ChoiceConditions.Add("Mirror Image Thrice", mirrorIsValid);
        chooseWhichToActivate.ChoiceConditions.Add("Lunar Volley Twice", volleyIsValid);

        ChoicesNeeded.Add(chooseWhichToActivate);
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.First() is ConditionalOptionSelectChoice condOpt && condOpt.ChoiceMade && condOpt.ChoiceConditions[condOpt.ChosenOption](this))
        {
            if (condOpt.ChosenOption == "Mirror Image Thrice")
            {
                for (int i = 0; i < 3; i++)
                {
                    ChoiceManager.TriggerBasicPlayerDecision(_mirrorAbil);
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    ChoiceManager.TriggerBasicPlayerDecision(_volleyAbil);
                }
            }
        }
    }

    public override void UpdateDescription()
    {
        Description = "Choose one: use Lunar Volley twice, or use Mirror Image three times.";
    }
}
