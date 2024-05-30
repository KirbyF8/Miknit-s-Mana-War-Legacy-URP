using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class LvlUp : MonoBehaviour
{
    
   
    int randomNumber;

    private int[] statsBLVL;

    private UIBattle uiBattle;


    private void Start()
    {
        uiBattle = FindObjectOfType<UIBattle>();
        
    }

    public void LevelUpStat(ref int[] stats, int[] scale)
   {

        ObtainUiBattle();
        
        // F*** Y******* 
        statsBLVL = StartStats(stats);
       

        randomNumber = Random.Range(1, 101);
        //Debug.Log(statsToLevelUp.Length);
        
        for (int i = 0; i < stats.Length; i++)
        {
            int randomNumber = Random.Range(1, 101);
            Debug.Log(statsBLVL);

            while (scale[i] >= 100)
            {
                stats[i]++;
                scale[i] -= 100;
                
                
            }

            if( randomNumber < scale[i] )
            {
                
                stats[i]++;
                

            }

            
           
        }
        uiBattle.LVLup(stats, statsBLVL);
    }

    private int[] StartStats(int[] stats)
    {
        return stats;
    }

    public void ObtainUiBattle() 
    { 
    uiBattle = FindObjectOfType<UIBattle>();
    }
}
