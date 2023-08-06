using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseInfoPanel : MonoBehaviour
{
    private TextMeshPro nameText;
    private TextMeshPro statsText;
    private TextMeshPro abilText;
    private int curAbilIndex = 0;
    private Creature myCreature;


    // Start is called before the first frame update
    void Start()
    {
        InitTextObjs();
    }

    public void SetCreature(Creature creat)
    {
        InitTextObjs();
        myCreature = creat;
        nameText.text = creat.DisplayName;
        statsText.text = "S: " + creat.Speed + "  I: " + creat.Initiative + "  H: " + creat.Health + "/" + creat.MaxHealth + "  A: " + creat.Attack;
        if(creat.Abilities.Count > 0)
        {
            abilText.text = creat.Abilities[0].DisplayName;
            curAbilIndex = 0;
        }
    }

    public void AbilChange(bool isLeft)
    {
        if (isLeft && curAbilIndex > 0)
        {
            curAbilIndex -= 1;
            abilText.text = myCreature.Abilities[curAbilIndex].DisplayName;
        }
        if(!isLeft && curAbilIndex < myCreature.Abilities.Count - 1)
        {
            curAbilIndex += 1;
            abilText.text = myCreature.Abilities[curAbilIndex].DisplayName;
        }
    }

    private void InitTextObjs()
    {
        if (nameText == null || statsText == null || abilText == null)
        {
            nameText = transform.Find("NameText").GetComponent<TextMeshPro>();
            statsText = transform.Find("StatsText").GetComponent<TextMeshPro>();
            abilText = transform.Find("AbilityText").GetComponent<TextMeshPro>();
        }
    }
}
