using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDemoSquare : MonoBehaviour
{
    public GridSpace MyGridSpace;
    public GameDemo MyGameDemo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseDown()
    {
        Creature selectedCreature = MyGameDemo.CurSelectedCreat;

        if (selectedCreature != null)
        {
            Player currentPlayer = selectedCreature.Controller;
            Game game = currentPlayer.MyGame;

            if (GameUtils.CanCallCreatureToGridSpace(selectedCreature, currentPlayer, game, MyGridSpace))
            {
                game.CallCharacter(selectedCreature, MyGridSpace, currentPlayer);
                //MyGameDemo.PotentialDeselect();
            }
        }
    }
}
