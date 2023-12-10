using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class AuraOfCourageAbility : MyTurnEndPassive
{
    private int _courageRange = 2;

    public AuraOfCourageAbility()
    {
        Name = "AuraOfCourage";
        DisplayName = "Aura of Courage";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        foreach (var creat in Owner.MyGame.AllCreatures.Where(x => x != Owner && x.Controller == Owner.Controller && GachaGrid.IsInRange(Owner, x, _courageRange)))
        {
            creat.AbilitiesTick();
        }
    }

    public override void UpdateDescription()
    {
        Description = "At the end of your turn, allies in Range " + _courageRange + " reduce their cooldown counters by an additional 1.";
    }

    public override void RankUpToOne()
    {
        _courageRange++;
    }

    public override void RankUpToTwo()
    {
        _courageRange++;
    }
}
