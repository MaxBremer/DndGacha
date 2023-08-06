using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CharacterRaritySelector
{
    public static readonly int[] DefaultRarityOdds = new int[] 
    {
        45, // COMMON
        30, // RARE
        15, // EPIC
        7, // LEGENDARY
        1,  // MYTHICAL
    };

    public static Rarity SelectRarityDefault()
    {
        return SelectRarityCustom(DefaultRarityOdds);
    }

    public static Rarity SelectRarityCustom(int[] distribution)
    {
        int selectedIndex = 0;
        int total = distribution.Sum();
        int randy = Random.Range(0, total + 1);

        for (int i = 0; i < distribution.Length; i++)
        {
            if(randy < distribution[i])
            {
                selectedIndex = i;
                break;
            }
            else
            {
                randy -= distribution[i];
            }
        }

        return (Rarity)selectedIndex;
    }
}


