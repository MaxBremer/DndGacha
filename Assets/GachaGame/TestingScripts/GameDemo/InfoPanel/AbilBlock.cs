using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilBlock : MonoBehaviour
{
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
        if(MyAbility is ActiveAbility activeAbil && activeAbil.IsActivateable())
        {
            //Debug.Log("Activating");
            activeAbil.Activate();
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

            if (!ChoiceManager.ValidChoicesExist(activeAbil2.ChoicesNeeded))
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
}
