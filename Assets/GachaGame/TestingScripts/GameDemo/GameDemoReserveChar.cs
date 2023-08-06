using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDemoReserveChar : MonoBehaviour
{
    public Creature MyCreature;
    public GameDemo MyGameDemo;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
