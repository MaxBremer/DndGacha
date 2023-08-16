using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDemoSquare : MonoBehaviour
{
    public GridSpace MyGridSpace;
    public GameDemo MyGameDemo;
    public bool Highlighted = false;
    public bool IsPathTarget = false;
    public bool IsPotentialPointTarget = false;
    public Color BaseColor;
    public Color IsPathColor;
    public Color HighlightColor;
    public Color AbilTargetColor = Color.magenta;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseDown()
    {
        Creature selectedCreature = MyGameDemo.CurSelectedCreat;

        if (selectedCreature != null)
        {
            if (IsPotentialPointTarget)
            {
                MyGameDemo.TriggerPointAbil(MyGridSpace);
                return;
            }
            if (selectedCreature.InReserve)
            {
                Player currentPlayer = selectedCreature.Controller;
                Game game = currentPlayer.MyGame;

                if (GameUtils.CanCallCreatureToGridSpace(selectedCreature, currentPlayer, game, MyGridSpace))
                {
                    game.CallCharacter(selectedCreature, MyGridSpace, currentPlayer);
                    //MyGameDemo.PotentialDeselect();
                }
            }else if(selectedCreature.IsOnBoard && IsPathTarget)
            {
                MyGameDemo.MyGame.GameGrid.Move(selectedCreature, MyGameDemo.GetPathTo(MyGridSpace), MyGridSpace);
            }
        }
    }

    void OnMouseEnter()
    {
        if (Highlighted && !IsPotentialPointTarget)
        {
            IsPathTarget = true;
            List<GridSpace> path = MyGameDemo.GetPathTo(MyGridSpace);
            foreach (var space in path)
            {
                MyGameDemo.GridObjs[space].square.PathHighlight();
            }
        }
    }

    void OnMouseExit()
    {
        if (IsPathTarget && !IsPotentialPointTarget)
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
        Highlighted = true;
        SetColor(HighlightColor);
    }

    public void PathHighlight()
    {
        SetColor(IsPathColor);
    }

    public void UnHighlight()
    {
        Highlighted = false;
        IsPathTarget = false;
        IsPotentialPointTarget = false;
        SetColor(BaseColor);
    }

    public void HighlightAbilityTarget()
    {
        IsPotentialPointTarget = true;
        SetColor(AbilTargetColor);
    }

    public void SetColor(Color c)
    {
        GetComponent<Renderer>().material.color = c;
    }
}
