using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDemoBoardChar : MonoBehaviour
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

    void OnMouseDown()
    {
        MyGameDemo.SelOnboardChar(gameObject);
    }

    public void Select()
    {
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }
        rend.material.color = selectedColor;

    }

    public void Deselect()
    {
        if(rend == null)
        {
            rend = GetComponent<Renderer>();
        }
        rend.material.color = defaultColor;
    }

    public void SetCreat(Creature creat)
    {
        MyCreature = creat;
    }
}
