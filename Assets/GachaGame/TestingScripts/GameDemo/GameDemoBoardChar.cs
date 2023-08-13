using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDemoBoardChar : MonoBehaviour
{
    public Creature MyCreature;
    public GameDemo MyGameDemo;

    public Color defaultColor = Color.white;
    public Color selectedColor = Color.yellow;
    public Color attackTargetColor = Color.red;

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
        if (MyGameDemo.ValidAttackTargets.Contains(MyCreature))
        {
            MyGameDemo.AttackTarget(MyCreature);

            return;
        }
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
        RevertHighlightToBase();
    }

    public void SetCreat(Creature creat)
    {
        MyCreature = creat;
    }

    public void HighlightAttackTarget()
    {
        rend.material.color = attackTargetColor;
    }

    public void RevertHighlightToBase()
    {
        rend.material.color = defaultColor;
    }
}
