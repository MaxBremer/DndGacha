using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TurnLog
{
    public int TurnNumber;
    public int InitiativeCount;
    public bool IsInitiativeTurn;
    public List<List<GameEvent>> TurnEventsByPlayer = new List<List<GameEvent>>();
    public List<GameEvent> AllEventsThisTurn 
    { 
        get
        {
            var returner = new List<GameEvent>();
            TurnEventsByPlayer.ForEach(x => returner.AddRange(x));
            return returner;
        }
    }
}
