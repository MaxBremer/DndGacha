using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MyTurnStartPassive : TurnStartPassive
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is TurnStartArgs turnArgs && turnArgs.PlayerWhoseTurnIsStarting == Owner.Controller.MyPlayerIndex)
        {
            ExternalTrigger(sender, e);
        }
    }
}
