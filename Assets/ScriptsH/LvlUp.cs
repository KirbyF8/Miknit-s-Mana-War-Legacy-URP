using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlUp : MonoBehaviour
{
    private Character characters;
    int randomNumber;

    [SerializeField]private int[] statsToLevelUp;


    private void Start()
    {
        characters = GetComponent<Character>();
        

        //Debug.Log(characters.HP);
        
    }

    // TODO: Acortar esto
    public void LevelUpStat(int hp, int hp_max, int hpScale, int mp, int mp_max, int mpScale,
    int strenght, int strenghtScale, int magic, int magicScale,
    int defense, int defenseScale, int resistance, int resistanceScale,
    int speed, int speedScale, int dexterity, int dexterityScale,
    int luck, int luckScale, int movement)
    {
        
        //?      Intentar llenar la array sin tener que traelo de la funcion     int[] statsToLevelUp; 
        
        statsToLevelUp = new int[] {
        hp_max, mp_max, strenght,
        magic, defense, resistance,
        speed, dexterity, luck
        };
       

        int[] scales = {
        hpScale, mpScale, strenghtScale,
        magicScale, defenseScale, resistanceScale,
        speedScale, dexterityScale, luckScale
    };

        //Debug.Log(statsToLevelUp[0]);

        randomNumber = Random.Range(1, 101);
        //Debug.Log(statsToLevelUp.Length);
        for (int i = 0; i < statsToLevelUp.Length; i++)
        {
            int randomNumber = Random.Range(1, 101);
            if( randomNumber < scales[i])
            {
                statsToLevelUp[i]++;
               
            }
            //? WHY?
            Debug.Log(statsToLevelUp[i]);
        }

    }
}
