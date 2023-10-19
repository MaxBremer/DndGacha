using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AbilityCreatureTargetSelectingArgs : EventArgs
{
    public Ability AbilityMakingChoice;
    public CreatureTargetChoice ChoiceBeingMade;
    public List<Creature> CurrentTargetsBeingConsidered;
}
