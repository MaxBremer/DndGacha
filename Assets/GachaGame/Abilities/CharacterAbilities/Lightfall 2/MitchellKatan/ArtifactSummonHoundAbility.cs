using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ArtifactSummonHoundAbility : TurnEndPassive
{
    public ArtifactSummonHoundAbility()
    {
        Name = "ArtifactSummonHound";
        DisplayName = "Undead Legions";
        Description = "At the end of your turn, summon a 3/1/4 unhead hound adjacent to this if there is space.";
    }

    public override void ConditionalTrigger(object sender, EventArgs e)
    {
        if (e is TurnEndArgs turnArgs && turnArgs.PlayerWhoseTurnIsEnding == Owner.Controller.MyPlayerIndex && Owner.MyGame.GameGrid.UnblockedAdjacentsExist(Owner.MySpace, false))
        {
            ExternalTrigger(sender, e);
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        var validSpaces = Owner.MyGame.GameGrid.GetUnblockedAdjacents(Owner.MySpace, false);
        var r = new Random();
        if(validSpaces.Count > 0)
        {
            Owner.MyGame.SummonCreature(GetHound(), validSpaces[r.Next(0, validSpaces.Count)]);
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
