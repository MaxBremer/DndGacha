using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameEvent
{
    public GachaEventType EventType;

    //TODO: Define some kind of data structure that says what events have what data and where.
    public List<string> StringData = new List<string>();
    public List<int> IntData = new List<int>();
    public List<Player> PlayerData = new List<Player>();
    public List<Creature> CreatureData = new List<Creature>();

    public GameEvent(GachaEventType myType)
    {
        EventType = myType;
    }

    public override string ToString()
    {
        string ret = string.Empty;
        switch (EventType)
        {
            case GachaEventType.StartOfTurn:
                ret += "Turn " + IntData[0] + " (Player " + IntData[1] + ") starting.\n";
                break;

            case GachaEventType.AfterAbilityTrigger:
                ret += "Ability '" + StringData[0] + "' triggered!\n";
                break;

            default:
                ret = "UNRECOGNIZED EVENT TYPE";
                break;
        }

        return ret;
    }
}
