using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class Battle : MonoBehaviour
{
    [SerializeField] private Character attacker;
   
    [SerializeField] private Character defender;
   
    [SerializeField] private UIBattle uiBattle;

    private VisualBattleV2 visualBattle;
    

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
        visualBattle = GetComponent<VisualBattleV2>();
        

        // visualBattle.SpawnCharacters(attacker, defender);
        // Combat();

        // uiBattle.ValueChanges(attacker, defender, aDMG, aATKs, aHit, aCrit, dDMG, dATKs, dHit, dCrit, weaponTriangle);
        // GetAllAttack();
        
    }


    public void updateUIBattle(Character battler1, Character battler2)
    {
       
        attacker = battler1;
        defender = battler2;
        Combat();
        //GetAllAttack();
        uiBattle.ValueChanges(attacker, defender, aDMG, aATKs, aHit, aCrit, dDMG, dATKs, dHit, dCrit, weaponTriangle);
    }

    public void GetAllAttack()
    {
        visualBattle.GetAllAttackCosas(attacker, defender, aCrit, dCrit, aDMG, dDMG, aATKs, dATKs, aHit, dHit);
    }
    private void Combat()
    {
        SetWeaponTriangle();
        NumberOfAttacks();

        DamaegeOfFisicAttack();
        DamaegeOfMagicAttack();
        HitChance();
        CritChance();

        /*
        Debug.Log("Tu daño = " + aDMG);      
        Debug.Log("Tu Nº de ataques = " + aATKs);
       Debug.Log("Tu % De golepar = " + aHit);
       Debug.Log("Tu % De crítico = "+ aCrit);


        Debug.Log("Su daño = " + dDMG);
        Debug.Log("Su Nº de ataques = " + dATKs);
        Debug.Log("Su % De golepar = " + dHit);
        Debug.Log("Su % De crítico = " + dCrit);
*/    
        }
        
    public int CalculateDMG(Character enemy, Character ally)
    {
        
        attacker = enemy;
        defender = ally;

        
        int aux = 0;

        NumberOfAttacks();

        if (attacker.dmgType) DamaegeOfMagicAttack();
        else DamaegeOfFisicAttack();

        aux += aDMG * aATKs;

        
        aux = (aux * 100) / defender.hp;
        /*
        if (aHit < 25)
        {
            aux -= 75;
        }
        else
        {
            aux += aHit / 2;
        }*/
        // Debug.Log(aux + defender.name);
        return aux;
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

    int dmgBeforeThings;
    int dmgBeforeThings2;
    int weaponTriangle;
    bool counterWeapon;

    int accuracyTriangle = 20;
    private void DamaegeOfFisicAttack()
    {
        
      

        if (attacker.dmgType == false) 
        {
            dmgBeforeThings = 0;
           
            if (counterWeapon == true)
            {
                dmgBeforeThings = attacker.stats[2];

               
            }

            if (weaponTriangle == 1)
            {
                aDMG = dmgBeforeThings + attacker.stats[2] - (defender.stats[5] / 2);
            }
            else if (weaponTriangle == 2)
            {
                aDMG = dmgBeforeThings + attacker.stats[2] / 2 - defender.stats[5];
            }
            else
            {
                aDMG = dmgBeforeThings + attacker.stats[2] - defender.stats[5];

            }

            if (aDMG < 0)
            {
                aDMG = 0;
            }




        }


        if (defender.dmgType == false)
        {
            dmgBeforeThings2 = 0;

            if (counterWeapon == true)
            {
                dmgBeforeThings2 = defender.stats[2];

                
            }

            if (weaponTriangle == 2)
            {
                dDMG = dmgBeforeThings2 + defender.stats[2] - (attacker.stats[5] / 2);
            }
            else if (weaponTriangle == 1)
            {

                dDMG = dmgBeforeThings2 + defender.stats[2] / 2 - attacker.stats[5];
            }
            else
            {
                dDMG = dmgBeforeThings2 + defender.stats[2] - attacker.stats[5];
            }

            if (dDMG < 0)
            {
                dDMG = 0;
            }

        }

    }

    private void DamaegeOfMagicAttack()
    {
        
        if (attacker.dmgType == true)
        {
            aDMG = attacker.stats[4] - defender.stats[6];

            if (aDMG < 0)
            {
                aDMG = 0;
            }
        }

        if (defender.dmgType == true)
        {
            dDMG = defender.stats[4] - attacker.stats[6];

            if (dDMG < 0)
            {
                dDMG = 0;
            }
        }

    }

    private void SetWeaponTriangle()
    {
        
        if (attacker.arma == "Espada" && defender.arma == "Hacha" || attacker.arma == "Hacha" && defender.arma == "Lanza" || attacker.arma == "Lanza" && defender.arma == "Espada")
        {
            weaponTriangle = 1;
        }
        else if (attacker.arma == "Espada" && defender.arma == "Lanza" || attacker.arma == "Hacha" && defender.arma == "Espada" || attacker.arma == "Lanza" && defender.arma == "Hacha")
        {
            weaponTriangle = 2;
        }
        else
        {
            
            weaponTriangle = 0;
        }
    }

    private void HitChance()
    {
        if (weaponTriangle == 1) { aHit += accuracyTriangle; }else if (weaponTriangle == 2) { aHit -= accuracyTriangle; };
        aHit = 100 - (defender.stats[3] - attacker.stats[3]) - (defender.stats[7] - attacker.stats[7]);
        if (aHit >= 100)
        {
            aHit = 100;
        }
        else if (aHit < 0)
        {
            aHit = 0;
        }
        if (weaponTriangle == 1) { dHit += accuracyTriangle; } else if (weaponTriangle == 2) { dHit -= accuracyTriangle; };
        dHit = 100 - (attacker.stats[3] - defender.stats[3]) - (attacker.stats[7] - defender.stats[7]);
        if (dHit >= 100)
        {
            dHit = 100;
        }
        else if(dHit < 0)
        {
            dHit = 0;
        }
    }
    
    private void CritChance()
    {

        aCrit = ((attacker.stats[8] * 2 ) - defender.stats[8] + (attacker.stats[3] - defender.stats[3]));

        if (aCrit >= 100)
        {
            aCrit = 100;
        }
        else if (aCrit < 0)
        {
            aCrit = 0;
        }

        

       

        dCrit = ((defender.stats[8] * 2) - attacker.stats[8] + (defender.stats[3] - attacker.stats[3]));

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
