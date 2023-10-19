using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AGlassStatueAbility : ActiveAbility
{
    public AGlassStatueAbility()
    {
        Name = "AGlassStatue";
        DisplayName = "A Glass Statue";
        Description = "If this is the only creature you control, you have no characters in reserve, and your opponent controls 3 or more creatures, you win the battle.";
        MaxCooldown = 0;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        var opp = Owner.MyGame.Players.Where(x => x != Owner.Controller).First();
        if(Owner.Controller.OnBoardCreatures.Count == 1 && Owner.Controller.OnBoardCreatures.First() == Owner && Owner.Controller.Reserve.Count == 0 && opp.OnBoardCreatures.Count >= 3)
        {
            Owner.MyGame.Win(Owner.Controller);
        }
    }
}
