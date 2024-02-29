using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GainControlArgs : EventArgs
{
    public Creature CreatureControlChanging;
    public Player OldController;
    public Player NewController;
}
