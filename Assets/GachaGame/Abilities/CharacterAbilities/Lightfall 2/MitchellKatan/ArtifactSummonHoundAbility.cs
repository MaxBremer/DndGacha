using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ArtifactSummonHoundAbility : TurnEndPassive
{
    private int _numHounds;

    public ArtifactSummonHoundAbility(int numHounds = 1)
    {
        Name = "ArtifactSummonHound";
        DisplayName = "Undead Legions";
        _numHounds = numHounds;
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is TurnEndArgs turnArgs && turnArgs.PlayerWhoseTurnIsEnding == Owner.Controller.MyPlayerIndex && Owner.MyGame.GameGrid.NumUnblockedSpacesExist(_numHounds))
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void UpdateDescription()
    {
        Description = "At the end of your turn, summon " + (_numHounds == 1 ? "a 3/1/4 unhead hound at the nearest available space." : _numHounds + " 3/1/4 undead hounds at the nearest available spaces.");
    }

    public override void Trigger(object sender, EventArgs e)
    {
        /*var validSpaces = Owner.MyGame.GameGrid.GetUnblockedAdjacents(Owner.MySpace, false);
        var r = new Random();
        if(validSpaces.Count > 0)
        {
            Owner.MyGame.SummonCreature(GetHound(), validSpaces[r.Next(0, validSpaces.Count)]);
        }*/
        for (int i = 0; i < _numHounds; i++)
        {
            var targetSpace = Owner.MyGame.GameGrid.GetNearestAvailableSpace(Owner.MySpace);
            if (targetSpace != null)
            {
                Owner.MyGame.SummonCreature(GetHound(), targetSpace);
            }
        }
    }

    private Creature GetHound()
    {
        var cbase = new CreatureGameBase()
        {
            Initiative = 1,
            Speed = 3,
            Health = 1,
            Attack = 4,
            DisplayName = "Undead Hound",
        };
        cbase.CreatureTypes.Add("Animal");
        cbase.CreatureTypes.Add("Undead");

        var creat = new Creature(cbase);
        creat.InitFromBase();
        creat.SetController(Owner.Controller);

        return creat;
    }
}
