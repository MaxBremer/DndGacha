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
    public Color abilityTargetColor = Color.magenta;

    public BoardCharSelMode MySelectMode = BoardCharSelMode.NORMAL;

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
        switch (MySelectMode)
        {
            case BoardCharSelMode.NORMAL:
                MyGameDemo.SelOnboardChar(gameObject);
                break;
            case BoardCharSelMode.ATTACK:
                MyGameDemo.AttackTarget(MyCreature);
                break;
            case BoardCharSelMode.ABIL:
                MyGameDemo.TriggerCreatureAbil(MyCreature);
                break;
            case BoardCharSelMode.NONE:
                break;
        }
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
        MySelectMode = BoardCharSelMode.ATTACK;
    }

    public void HighlightAbilTarget()
    {
        rend.material.color = abilityTargetColor;
        MySelectMode = BoardCharSelMode.ABIL;
    }

    public void RevertHighlightToBase()
    {
        rend.material.color = defaultColor;
        MySelectMode = BoardCharSelMode.NORMAL;
    }
}

public enum BoardCharSelMode
{
    NORMAL,
    ATTACK,
    ABIL,
    NONE,
}