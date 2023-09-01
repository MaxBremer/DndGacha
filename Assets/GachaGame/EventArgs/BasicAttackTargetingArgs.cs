using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BasicAttackTargetingArgs : EventArgs
{
    public List<Creature> ValidAttackTargets = new List<Creature>();
    public Creature CreatureAttacking = null;
}
