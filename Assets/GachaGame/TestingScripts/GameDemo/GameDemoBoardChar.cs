using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [SerializeField]
    private TextMeshPro nameTextObj;

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
        SetColor(selectedColor);

    }

    public void Deselect()
    {
        RevertHighlightToBase();
    }

    public void SetCreat(Creature creat)
    {
        MyCreature = creat;
        nameTextObj.text = creat.DisplayName;
    }

    public void HighlightAttackTarget()
    {
        SetColor(attackTargetColor);
        MySelectMode = BoardCharSelMode.ATTACK;
    }

    public void HighlightAbilTarget()
    {
        SetColor(abilityTargetColor);
        MySelectMode = BoardCharSelMode.ABIL;
    }

    public void RevertHighlightToBase()
    {
        SetColor(defaultColor);
        MySelectMode = BoardCharSelMode.NORMAL;
    }

    private void SetColor(Color col)
    {
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }
        rend.material.color = col;
    }
}

public enum BoardCharSelMode
{
    NORMAL,
    ATTACK,
    ABIL,
    NONE,
}