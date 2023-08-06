using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDemoSquare : MonoBehaviour
{
    public GridSpace MyGridSpace;
    public GridDemo MyGridDemo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsObstacle => MyGridSpace.Obstacle;

    public bool IsMovableTo = false;

    public bool MouseIsOver = false;

    public bool IsPath = false;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MyGridSpace.Obstacle = !IsObstacle;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            MyGridDemo.xPos = MyGridSpace.XPos;
            MyGridDemo.yPos = MyGridSpace.YPos;
        }
    }

    private void OnMouseEnter()
    {
        MouseIsOver = true;
    }

    private void OnMouseExit()
    {
        MouseIsOver = false;
    }
}
