using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class LvlUp : MonoBehaviour
{
    
   
    int randomNumber;

    

   


    public void LevelUpStat(ref int[] stats, int[] scale)
   {
       

        randomNumber = Random.Range(1, 101);
        //Debug.Log(statsToLevelUp.Length);
        for (int i = 0; i < stats.Length; i++)
        {
            int randomNumber = Random.Range(1, 101);

             
            while (scale[i] >= 100)
            {
                stats[i]++;
                scale[i] -= 100;
                
            }

            if( randomNumber < scale[i] )
            {
                stats[i]++;
               
            }
            
            Debug.Log(stats[i]);
        }

    }
}
