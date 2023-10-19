using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AbilityActivateArgs : EventArgs
{
    public ActiveAbility AbilityActivating;
    public int NumActivations = 1;
    public bool CounterActivation = false;
}
