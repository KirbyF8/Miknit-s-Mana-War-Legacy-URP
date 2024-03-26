using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlUp : MonoBehaviour
{
    private Character character;
    int randomNumber;


    private void Start()
    {
        character = GetComponent<Character>();
        
    }

    public void LevelUpStat()
    {


        int[] statsToLevelUp; statsToLevelUp = new int[] {
        character.HP_max, character.MP_max, character.Strenght,
        character.Magic, character.Defense, character.Resistance,
        character.Speed, character.Dexterity, character.Luck
        };

        int[] scales = {
        character.HPScale, character.MPScale, character.StrenghtScale,
        character.MagicScale, character.DefenseScale, character.ResistanceScale,
        character.SpeedScale, character.DexterityScale, character.LuckScale
    };

        Debug.Log(statsToLevelUp[0]);

        randomNumber = Random.Range(1, 101);
        for (int i = 0; i < statsToLevelUp.Length; i++)
        {
            int randomNumber = Random.Range(1, 101);
            if( randomNumber < scales[i])
            {
                statsToLevelUp[i]++;
                Debug.Log(statsToLevelUp[i]);
            }
        }

    }
}
