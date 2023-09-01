using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilBlock : MonoBehaviour
{
    public bool AllowInteraction = true;
    public GameObject InfoBoxGameObject;
    public TextMeshPro InfoBoxText;
    public DragDrop PanelDragDrop;

    public BaseInfoPanel MyInfoPanel;

    public Ability MyAbility;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if(AllowInteraction && MyAbility is ActiveAbility activeAbil && activeAbil.IsActivateable())
        {
            //Debug.Log("Activating");
            activeAbil.Activate();
            if(activeAbil.ChoicesNeeded.Count < 1 && ChoiceManager.AbilityPending == null)
            {
                MyInfoPanel.MyGameDemo.PotentialDeselect();
            }
        }

        /*if (MyAbility is ActiveAbility activeAbil2 && !activeAbil2.IsActivateable())
        {
            if (!activeAbil2.Owner.CanAct)
            {
                Debug.Log("Owner can't act");
            }

            if (activeAbil2.Cooldown > 0)
            {
                Debug.Log("Cooldown is greater than zero");
            }

            if (activeAbil2.Cooldown < 0)
            {
                Debug.Log("COOLDOWN SOMEHOW LESS THAN ZERO");
            }

            if (!ChoiceManager.ValidChoicesExist(activeAbil2.ChoicesNeeded, activeAbil2))
            {
                Debug.Log("Not enough valid choices");
            }
        }

        if (MyAbility == null)
        {
            Debug.Log("MY ABILITY IS NULL");
        }
        else
        {
            Debug.Log("Ability name: " + MyAbility.DisplayName);
        }*/
    }

    void OnMouseEnter()
    {
        if (!PanelDragDrop.IsDragging)
        {
            ShowTooltip();
        }
    }

    void OnMouseExit()
    {
        HideTooltip();
    }

    private void ShowTooltip()
    {
        string tooltipContent = MyAbility.DisplayName + "\n" + MyAbility.Description;

        if (MyAbility is ActiveAbility activeAbil)
        {
            tooltipContent += "\nCooldown: " + activeAbil.Cooldown + "/" + activeAbil.MaxCooldown;
        }

        InfoBoxText.text = tooltipContent;
        InfoBoxGameObject.SetActive(true);
    }

    private void HideTooltip()
    {
        InfoBoxGameObject.SetActive(false);
    }
}
