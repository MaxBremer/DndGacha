using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AuraOfCourageAbility : MyTurnEndPassive
{
    public AuraOfCourageAbility()
    {
        Name = "AuraOfCourage";
        DisplayName = "Aura of Courage";
        Description = "At the end of your turn, allies in Range 2 reduce their cooldown counters by an additional 1.";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        foreach (var creat in Owner.MyGame.AllCreatures.Where(x => x != Owner && x.Controller == Owner.Controller && GachaGrid.IsInRange(Owner, x, 2)))
        {
            creat.AbilitiesTick();
        }
    }
}
