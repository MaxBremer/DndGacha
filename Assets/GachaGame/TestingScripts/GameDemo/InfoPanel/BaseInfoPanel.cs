using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseInfoPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject AbilityBlockObject;
    [SerializeField]
    private Color ActiveAbilColor;
    [SerializeField]
    private Color PassiveAbilColor;

    private TextMeshPro nameText;
    private TextMeshPro statsText;
    private TextMeshPro abilText;
    private int curAbilIndex = 0;
    private Creature myCreature;

    public GameDemo MyGameDemo;


    // Start is called before the first frame update
    void Start()
    {
        InitTextObjs();
    }

    public void SetCreature(Creature creat, bool forbidInteraction)
    {
        InitTextObjs();
        myCreature = creat;
        nameText.text = creat.DisplayName;
        statsText.text = "S: " + creat.Speed + "  I: " + creat.Initiative + "  H: " + creat.Health + "/" + creat.MaxHealth + "  A: " + creat.Attack;
        if(creat.Abilities.Count > 0)
        {
            curAbilIndex = 0;
            UpdateAbilityBlock();
        }

        if (forbidInteraction)
        {
            AbilityBlockObject.GetComponent<AbilBlock>().AllowInteraction = false;
        }
    }

    public void AbilChange(bool isLeft)
    {
        if (isLeft && curAbilIndex > 0)
        {
            curAbilIndex -= 1;
        }
        if(!isLeft && curAbilIndex < myCreature.Abilities.Count - 1)
        {
            curAbilIndex += 1;
        }
        UpdateAbilityBlock();
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

    private void UpdateAbilityBlock(int abilityIndex = -1)
    {
        abilityIndex = abilityIndex < 0 ? curAbilIndex : abilityIndex;

        abilText.text = myCreature.Abilities[abilityIndex].DisplayName;

        // Check if the ability is Active or Passive and update the color accordingly
        if (myCreature.Abilities[abilityIndex] is ActiveAbility)
        {
            // Set color for ActiveAbility (example: red)
            AbilityBlockObject.GetComponent<Renderer>().material.color = ActiveAbilColor;
        }
        else if (myCreature.Abilities[abilityIndex] is PassiveAbility)
        {
            // Set color for PassiveAbility (example: blue)
            AbilityBlockObject.GetComponent<Renderer>().material.color = PassiveAbilColor;
        }

        AbilityBlockObject.GetComponent<AbilBlock>().MyAbility = myCreature.Abilities[abilityIndex];
    }
}
