using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private string Name = "Yuu";

    [SerializeField] private int lvl = 1;
    [SerializeField] private int exp;
    [SerializeField] private int exp_max;


    [SerializeField] private int hp = 8;

    [SerializeField] private int mp = 12;

    [SerializeField] private int movement = 6;

    // HP MP AD AP FR MR SP LK  

    [SerializeField] private int[] stats;


    [SerializeField] private int[] scale;

   


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
            lvl++;
            exp = exp - exp_max;
            exp_max += exp_max;

            lvlUp.LevelUpStat(ref stats, scale);
            
        }
    }
}
