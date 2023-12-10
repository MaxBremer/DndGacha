using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class AGlassStatueAbility : ActiveAbility
{
    private int numEnemyCreatures = 3;

    public AGlassStatueAbility()
    {
        Name = "AGlassStatue";
        DisplayName = "A Glass Statue";
        MaxCooldown = 0;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        var opp = Owner.MyGame.Players.Where(x => x != Owner.Controller).First();
        if(ShouldWinGame(opp))
        {
            Owner.MyGame.Win(Owner.Controller);
        }
    }

    public override void RankUpToOne()
    {
        numEnemyCreatures--;
    }

    public override void RankUpToTwo()
    {
        numEnemyCreatures--;
    }

    public override void UpdateDescription()
    {
        Description = "If this is the only creature you control, you have no characters in reserve, and your opponent controls " + numEnemyCreatures + " or more creatures, you win the battle.";
    }

    private bool ShouldWinGame(Player opp)
    {
        return Owner.Controller.OnBoardCreatures.Count == 1
            && Owner.Controller.OnBoardCreatures.First() == Owner
            && Owner.Controller.Reserve.Count == 0
            && opp.OnBoardCreatures.Count >= 3;
    }
}
