using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    [SerializeField] private Character attacker;
    [SerializeField] private Character defender;

    int aDMG = 0;
    int dDMG = 0;

    int aATKs = 1;
    int dATKs = 1;

    int aHit = 0;
    int dHit = 0;

    int aCrit = 0;
    int dCrit = 0;

    private void Start()
    {
        Combat();
    }

    private void Combat()
    {
        NumberOfAttacks();

        DamaegeOfFisicAttack();
        DamaegeOfMagicAttack();
        HitChance();
        CritChance();

        Debug.Log(aDMG);
      
        Debug.Log(aATKs);
       
        Debug.Log(aHit);
       
        Debug.Log(aCrit);   

    }

    private void NumberOfAttacks()
    {

        aATKs = attacker.stats[7] / defender.stats[7];
        if (aATKs <= 1)
        {
            aATKs = 1;
        }
        else if (aATKs >= 4)
        {
            aATKs = 4;
        }

        dATKs = defender.stats[7] / attacker.stats[7];
        if (dATKs <= 1)
        {
            dATKs = 1;
        }
        else if (dATKs >= 4)
        {
            dATKs = 4;
        }
    }
    
    private void DamaegeOfFisicAttack()
    {
        if (attacker.dmgType == false) 
        {
            aDMG = 10*(attacker.stats[2] * (defender.stats[5] / (attacker.stats[2] + defender.stats[5])));
        }


        if (attacker.dmgType == false)
        {
            dDMG = defender.stats[2] * (attacker.stats[2] / (defender.stats[2] + attacker.stats[5]));
        }

    }

    private void DamaegeOfMagicAttack()
    {
        if (defender.dmgType == true)
        {
            aDMG = attacker.stats[4] * (defender.stats[6] / (attacker.stats[4] + defender.stats[6]));
        }

        if (defender.dmgType == true)
        {
            dDMG = defender.stats[4] * (attacker.stats[6] / (defender.stats[4] + attacker.stats[6]));
        }

    }

    private void HitChance()
    {
        aHit = 100 - (defender.stats[3] - attacker.stats[3]) - (defender.stats[7] - attacker.stats[7]);
        if (aHit >= 100)
        {
            aHit = 100;
        }
        dHit = 100 - (attacker.stats[3] - defender.stats[3]) - (attacker.stats[7] - defender.stats[7]);
        if (dHit >= 100)
        {
            dHit = 100;
        }
    }

    private void CritChance()
    {
        aCrit = (attacker.stats[8] - defender.stats[8] + (attacker.stats[3] - defender.stats[3]));

        if (aCrit >= 100)
        {
            aCrit = 100;
        }
        else if (aCrit < 0)
        {
            aCrit = 0;
        }


        dCrit = (defender.stats[8] - attacker.stats[8] + (defender.stats[3] - attacker.stats[3]));

        if (dCrit >= 100)
        {
            dCrit = 100;
        }
        else if (dCrit < 0)
        {
            dCrit = 0;
        }
    }
}
