using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    
    private string Name = "Yuu";

    public int lvl = 1;
    public int exp;
    public int exp_max;


    public int hp = 8;

    public int mp = 12;

    public int movement = 6;

    // HP Mana Strenght Dexterity Magic Defense Resistance Speed Luck  

    public int[] stats;


    public int[] scale;


    public string arma;

    public bool dmgType;
    
    //? Hacha Lanza Espada Magia Arco

    private LvlUp lvlUp;
    
    void Start()
    {
        lvlUp = GetComponent<LvlUp>();
        //lvlUp = FindAnyObjectByType<LvlUp>();
        // ExpReset();
    }

    public void ExpReset()
    {
        if (exp >= exp_max)
        {
            lvl++;
            exp = exp - exp_max;
            exp_max += exp_max;

            lvlUp.LevelUpStat(ref stats, scale);
            
        }
    }
}
