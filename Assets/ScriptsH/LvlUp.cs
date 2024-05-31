using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class LvlUp : MonoBehaviour
{
    
   
    int randomNumber;

    private int[] statsBLVL;

    private UIBattle uiBattle;

    private int scaleSave;

    private void Start()
    {
        uiBattle = FindObjectOfType<UIBattle>();
        
    }

    public void LevelUpStat(ref int[] stats, int[] scale)
   {

        ObtainUiBattle();
        
        // F*** Y******* 
        statsBLVL = StartStats(stats);
        Debug.Log(statsBLVL[7]);

        randomNumber = Random.Range(1, 101);
        //Debug.Log(statsToLevelUp.Length);
        
        for (int i = 0; i < stats.Length; i++)
        {
            int randomNumber = Random.Range(1, 101);

            scaleSave = scale[i]; 

            while (scale[i] >= 100)
            {
                stats[i]++;
                scale[i] -= 100;
                
                
            }

            if( randomNumber < scale[i] )
            {
                
                stats[i]++;
                

            }

            scale[i] = scaleSave;
           
        }
        uiBattle.LVLup(stats, statsBLVL);
    }

    private int[] StartStats(int[] stats)
    {
        int[] arrayNuevo = new int[stats.Length]  ;
      

        for (int i = 0; i < stats.Length; i++)
        {
            arrayNuevo[i] = stats[i];
        }

        return arrayNuevo;
    }

    public void ObtainUiBattle() 
    { 
    uiBattle = FindObjectOfType<UIBattle>();
    }
}
