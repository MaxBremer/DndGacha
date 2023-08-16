using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDemoSquare : MonoBehaviour
{
    public GridSpace MyGridSpace;
    public GameDemo MyGameDemo;
    public SquareState MyState = SquareState.Base;
    public Color BaseColor;
    public Color IsPathColor;
    public Color HighlightColor;
    public Color AbilTargetColor = Color.magenta;

    void OnMouseDown()
    {
        Creature selectedCreature = MyGameDemo.CurSelectedCreat;

        if (selectedCreature != null)
        {
            switch (MyState)
            {
                case SquareState.Base:
                    if (selectedCreature.InReserve)
                    {
                        Player currentPlayer = selectedCreature.Controller;
                        Game game = currentPlayer.MyGame;

                        if (GameUtils.CanCallCreatureToGridSpace(selectedCreature, currentPlayer, game, MyGridSpace))
                        {
                            game.CallCharacter(selectedCreature, MyGridSpace, currentPlayer);
                        }
                    }
                    break;
                case SquareState.PathTargetHighlight:
                    MyGameDemo.MyGame.GameGrid.Move(selectedCreature, MyGameDemo.GetPathTo(MyGridSpace), MyGridSpace);
                    break;
                case SquareState.AbilTarget:
                    MyGameDemo.TriggerPointAbil(MyGridSpace);
                    break;
                case SquareState.NONE:
                default:
                    Debug.LogError("ERROR: Unrecognized square state clicked.");
                    break;
            }
        }
    }

    void OnMouseEnter()
    {
        if (MyState == SquareState.MoveHighlight)
        {
            List<GridSpace> path = MyGameDemo.GetPathTo(MyGridSpace);
            foreach (var space in path)
            {
                MyGameDemo.GridObjs[space].square.PathHighlight();
            }
            MyState = SquareState.PathTargetHighlight;
        }
    }

    void OnMouseExit()
    {
        if (MyState == SquareState.PathTargetHighlight)
        {
            List<GridSpace> path = MyGameDemo.GetPathTo(MyGridSpace);
            foreach (var space in path)
            {
                MyGameDemo.GridObjs[space].square.Highlight();
            }
        }
    }

    public void Highlight()
    {
        MyState = SquareState.MoveHighlight;
        SetColor(HighlightColor);
    }

    public void PathHighlight()
    {
        MyState = SquareState.PathHighlight;
        SetColor(IsPathColor);
    }

    public void UnHighlight()
    {
        MyState = SquareState.Base;
        SetColor(BaseColor);
    }

    public void HighlightAbilityTarget()
    {
        MyState = SquareState.AbilTarget;
        SetColor(AbilTargetColor);
    }

    public void SetColor(Color c)
    {
        GetComponent<Renderer>().material.color = c;
    }
}

public enum SquareState
{
    Base,
    MoveHighlight,
    PathTargetHighlight,
    PathHighlight,
    AbilTarget,
    NONE,
}
