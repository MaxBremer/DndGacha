using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDemo : MonoBehaviour
{
    private Game g;
    private Dictionary<GridSpace, GameObject> GridObjs = new Dictionary<GridSpace, GameObject>();

    public GameObject gridSquareObj;
    public Color P1CallSquareColor;
    public Color P2CallSquareColor;
    public Color PossibleMovesColor;
    public Color DefaultColor;
    public Color ObstacleColor;
    public Color PathColor;
    public int xPos, yPos, moveRange;

    // Start is called before the first frame update
    void Start()
    {
        g = TestProperty.SetupBasicTestGame(9, 0);
        g.Init();
        foreach (var gSquare in g.GameGrid.GetAllGridSquares())
        {
            var space = Instantiate(gridSquareObj, new Vector3(gSquare.XPos, gSquare.YPos, 0), Quaternion.identity);
            var demoComponent = space.GetComponent<GridDemoSquare>();
            demoComponent.MyGridSpace = gSquare;
            demoComponent.MyGridDemo = this;
            GridObjs.Add(gSquare, space);
        }
        ResetColors();
    }

    void Update()
    {
        ResetColors();
    }

    private void ResetColors()
    {
        var moveSquares = g.GameGrid.GetValidMoves(xPos, yPos, moveRange);

        foreach (var gSquare in g.GameGrid.GetAllGridSquares())
        {
            GridObjs[gSquare].GetComponent<Renderer>().material.color = DefaultColor;
            GridObjs[gSquare].GetComponent<GridDemoSquare>().IsPath = false;
        }

        foreach (var gSquare in g.GameGrid.GetAllGridSquares())
        {
            var mat = GridObjs[gSquare].GetComponent<Renderer>().material;
            var gridDemoSquare = GridObjs[gSquare].GetComponent<GridDemoSquare>();

            if (gridDemoSquare.IsPath)
            {
                continue;
            }

            if (moveSquares.ContainsKey(gSquare))
            {
                mat.color = PossibleMovesColor;
                if (gridDemoSquare.MouseIsOver)
                {
                    foreach (var sq in moveSquares[gSquare])
                    {
                        GridObjs[sq].GetComponent<Renderer>().material.color = PathColor;
                        GridObjs[sq].GetComponent<GridDemoSquare>().IsPath = true;
                    }
                }
            }

            if (g.Players[0].ValidInitSpaces.Contains(gSquare))
            {
                mat.color = P1CallSquareColor;
            }
            else if (g.Players[1].ValidInitSpaces.Contains(gSquare))
            {
                mat.color = P2CallSquareColor;
            }

            if (gSquare.Obstacle)
            {
                mat.color = ObstacleColor;
            }

            
        }
    }
}
