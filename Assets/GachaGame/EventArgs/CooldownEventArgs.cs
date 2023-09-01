using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CooldownEventArgs : EventArgs
{
    public int CooldownAmount = 0;
    public Ability AbilityCooled = null;
    public Creature AbilityOwner = null;
}
