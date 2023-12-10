using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameDemoReserveChar : MonoBehaviour
{
    public Creature MyCreature;
    public GameDemo MyGameDemo;

    public Color defaultColor = Color.white;
    public Color selectedColor = Color.yellow;
    public Color abilTargetColor = Color.magenta;

    private Renderer rend;

    [SerializeField]
    private TextMeshPro charNameText;

    private bool isAbilTarget = false;

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
        charNameText.text = creat.DisplayName;
    }

    private void OnMouseDown()
    {
        if (!isAbilTarget)
        {
            MyGameDemo.SelReserveChar(gameObject);
            rend.material.color = selectedColor;
        }
        else
        {
            MyGameDemo.TriggerCreatureAbil(MyCreature);
        }
        
    }

    public void HighlightAbilTarget()
    {
        rend.material.color = abilTargetColor;
        isAbilTarget = true;
    }

    public void Deselect()
    {
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }
        rend.material.color = defaultColor;
        isAbilTarget = false;
    }
}
