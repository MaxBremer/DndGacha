using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class StrategicSacrificeAbility : TargetNonselfFriendlyAbility
{
    public StrategicSacrificeAbility()
    {
        Name = "StrategicSacrifice";
        DisplayName = "Strategic Sacrifice";
        Description = "Choose a friendly creature. Choose one: Destroy it, or give control of it to your opponent.";
        MaxCooldown = 0;
    }

    public override void InitAbility()
    {
        base.InitAbility();

        var optSel = new OptionSelectChoice() { Caption = "Choice", Options = (new string[] { "Destroy creature", "Give control of creature to Opponent", }).ToList() };
        ChoicesNeeded.Add(optSel);
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade && ChoicesNeeded.Where(x => x.Caption == "Choice").FirstOrDefault() is OptionSelectChoice optChoice && optChoice.ChoiceMade)
        {
            if(optChoice.ChosenOption == "Destroy creature")
            {
                creatChoice.TargetCreature.Die();
            }
            else
            {
                var opp = Owner.MyGame.Players.Where(x => x != Owner.Controller).FirstOrDefault();
                if(opp != null)
                {
                    creatChoice.TargetCreature.SetController(opp);
                }
            }
        }
    }
}
