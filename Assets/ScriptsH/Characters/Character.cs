using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private string Name = "Yuu";

    public int Lvl = 1;
    public int exp;
    public int exp_max;



    public int HP = 8;
    public int HP_max = 8;
    public int HPScale = 50;

    public int MP = 12;
    public int MP_max = 12;
    public int MPScale = 50;

    public int Strenght = 12;
    public int StrenghtScale = 50;

    public int Magic = 8;
    public int MagicScale = 45;

    public int Defense = 7;
    public int DefenseScale = 20;

    public int Resistance = 12;
    public int ResistanceScale = 45;

    public int Speed = 18;
    public int SpeedScale = 95;

    public int Dexterity = 18;
    public int DexterityScale = 95;

    public int Luck = 1;
    public int LuckScale = 5;


    public int Movement = 6;

    private LvlUp lvlUp;
    
    void Start()
    {
        lvlUp = GetComponent<LvlUp>();
        //lvlUp = FindAnyObjectByType<LvlUp>();
        ExpReset();
    }

    public void ExpReset()
    {
        if (exp >= exp_max)
        {
            Lvl++;
            exp = exp - exp_max;
            exp_max += exp_max;

            lvlUp.LevelUpStat(HP, HP_max, HPScale, MP, MP_max, MPScale, Strenght, StrenghtScale, Magic,MagicScale,Defense,DefenseScale,Resistance,ResistanceScale,Speed,SpeedScale,Dexterity,DexterityScale,Luck,LuckScale,Movement);
            
        }
    }
}
