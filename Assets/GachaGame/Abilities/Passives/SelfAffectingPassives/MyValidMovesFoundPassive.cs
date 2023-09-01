using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MyValidMovesFoundPassive : CreatureMovesFoundWhileOnboardPassive
{
    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if(e is ValidMovesFoundForCreatArgs moveArgs && moveArgs.CreatureMoving == Owner)
        {
            ExternalTrigger(sender, e);
        }
    }
}
