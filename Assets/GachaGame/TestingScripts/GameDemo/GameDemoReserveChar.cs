using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDemoReserveChar : MonoBehaviour
{
    public Creature MyCreature;
    public GameDemo MyGameDemo;

    public Color defaultColor = Color.white;
    public Color selectedColor = Color.yellow;

    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCreat(Creature creat)
    {
        MyCreature = creat;
    }

    private void OnMouseDown()
    {
        MyGameDemo.SelReserveChar(gameObject);
        rend.material.color = selectedColor;
    }

    public void Deselect()
    {
        rend.material.color = defaultColor;
    }
}
