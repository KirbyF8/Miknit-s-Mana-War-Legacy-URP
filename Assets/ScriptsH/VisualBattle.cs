using System.Collections;
using System.Collections.Generic;
using System.Globalization;

using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class VisualBattle : MonoBehaviour
{
    private Vector3 attackerPos = new Vector3(600, 180, 42);

    private Vector3 defenderPos = new Vector3(600, 180, 52);

    [SerializeField] private UIBattle uiBattle;

    private string defenderName;
    private string attackerName;

    private int maxAttacks1;
    private int attacks1;
    private int maxAttacks2;
    private int attacks2;

    private string atackerNameConst;

    private bool starded = false;

    public void SpawnCharacters(Character attacker, Character defender)
    {
       atackerNameConst = attacker.name;

        //? Quitar mas tarde
        attacker.hp = attacker.stats[0];
        defender.hp = defender.stats[0];
        //? Quitar mas tarde

        Instantiate(attacker, attackerPos, Quaternion.Euler(0,0,0));
        Instantiate(defender, defenderPos, Quaternion.Euler(0, 180, 0));
    }

    private void A_Atack(Character atacker, Character defender, int aCrit, int aDMG, int aHit)
    {
        attackerName = atacker.name + "(Clone)";
        defenderName = defender.name + "(Clone)";

        int aux = Random.Range(0, 101);

       //if (!starded) { 

        if (atacker.hp <= 0 || defender.hp <= 0)
        {
            //? Dejenlo ya esta muerto
            return;
        }
        else
        {
            if (aux <= aHit)
            {
                if (aDMG <= 0)
                {
                    //! No Damage
                    Wait(atacker, defender, false, false, true, 0);
                    
                    return;
                }
                else
                {

                    
                        aux = Random.Range(0, 101);
                        if (aux < aCrit)
                        {
                            Wait(atacker, defender, false, true, false, aDMG * 3);
                           
                           
                            

                        }
                        else
                        {
                            Wait(atacker, defender, false, false, false, aDMG);
                            

                          
                            
                        }
                    
                }
            }
            else
            {
                //! miss
                Wait(atacker, defender, true, false, false, 0);
                
                
                return;
            }
        }
        //}
    }



    int auxDefenderHits = 0;
    int auxAttackerHits = 0;

    public void AnimationBattle(Character atacker, Character defender, int aCrit, int dCrit, int aDMG, int dDMG, int aATKs, int dATKs, int aHit, int dHit)
    {

        maxAttacks1 = aATKs;
        maxAttacks2 = dATKs;

       

        int aux = Random.Range(0, 101);
        

         while (auxAttackerHits < aATKs || auxDefenderHits < dATKs)
        {
             
            if ((aATKs-auxAttackerHits) >= 3 )
            {
                A_Atack(atacker, defender, aCrit, aDMG,  aHit);
                //auxAttackerHits++;
                uiBattle.HealthUpdate(atacker, defender);
                A_Atack(atacker, defender, aCrit, aDMG, aHit);
                //auxAttackerHits++;
                uiBattle.HealthUpdate(atacker, defender);
            }
            else
            {
                A_Atack(atacker, defender, aCrit, aDMG, aHit);
                auxAttackerHits++;
                uiBattle.HealthUpdate(atacker, defender);
            }

            if ((dATKs - auxDefenderHits) >= 3)
            {
                A_Atack(defender, atacker, dCrit, dDMG, dHit);
                auxDefenderHits++;
                uiBattle.HealthUpdate(atacker, defender);
                A_Atack(defender, atacker, dCrit, dDMG, dHit);
                auxDefenderHits++;
                uiBattle.HealthUpdate(atacker, defender);
            }
            else
            {
                A_Atack(defender, atacker, dCrit, dDMG, dHit);
                auxDefenderHits++;
                uiBattle.HealthUpdate(atacker, defender);
            }
            
        }
       
          
      
        
    }


    private void Wait(Character atacker, Character defender, bool missed, bool crit, bool noDMG, int aDMG)
    {
        GameObject AtackerBody = GameObject.Find(atacker.name + "(Clone)");
        GameObject DefenderBody = GameObject.Find(defender.name + "(Clone)");

        Animator atackerAnimator = AtackerBody.GetComponent<Animator>();
        Animator defenderAnimator = DefenderBody.GetComponent<Animator>();

        
            StartCoroutine("ADamage");
            
        
      
        
        
    }

    
    private IEnumerator ADamage()
    {

        starded = true;
        Character defender; 
        int aDMG = uiBattle.ReturnDamage();
       
        GameObject DefenderBody = GameObject.Find(defenderName);
     

        defender = DefenderBody.GetComponent<Character>();

        Character atacker;
        GameObject AtackerBody = GameObject.Find(attackerName);
        atacker = AtackerBody.GetComponent<Character>();
        Animator atackerAnimator = AtackerBody.GetComponent<Animator>();

        atackerAnimator.SetBool("AttackV2", true);
        Debug.Log(aDMG);
        yield return new WaitForSeconds(1.2f);
        atackerAnimator.SetBool("AttackV2", false);

        defender.hp -= aDMG;
        
        if (atacker.name == atackerNameConst)
        {
            uiBattle.HealthUpdate(atacker, defender);
        }
        else 
        {
            uiBattle.HealthUpdate(defender, atacker);
        }

        

       

        starded = false;

     

    }


}
